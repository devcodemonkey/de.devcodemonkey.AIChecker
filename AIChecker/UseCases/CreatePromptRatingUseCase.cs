﻿using de.devcodemonkey.AIChecker.CoreBusiness.DbModels;
using de.devcodemonkey.AIChecker.CoreBusiness.Interfaces;
using de.devcodemonkey.AIChecker.CoreBusiness.Models;
using de.devcodemonkey.AIChecker.DataSource.APIRequester.Interfaces;
using de.devcodemonkey.AIChecker.UseCases.Interfaces;
using de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;
using System.Globalization;
using System.Text.Json;

namespace de.devcodemonkey.AIChecker.UseCases
{
    public class CreatePromptRatingUseCase : ICreatePromptRatingUseCase
    {
        private readonly IDefaultMethodesRepository _defaultMethodesRepository;
        private readonly ILoadUnloadLms _loadUnloadLms;
        private readonly IAPIRequester _apiRequester;

        public delegate void StatusHandler(string statusMessage, Action action);

        public CreatePromptRatingUseCase(IDefaultMethodesRepository defaultMethodesRepository, ILoadUnloadLms loadUnloadLms, IAPIRequester aPIRequester)
        {
            _defaultMethodesRepository = defaultMethodesRepository;
            _loadUnloadLms = loadUnloadLms;
            _apiRequester = aPIRequester;
        }

        public async Task ExecuteAsync(PromptRatingUseCaseParams promptParams, Action<Result> displayResult, StatusHandler? statusHandler = null)
        {
            int round = 0;

            var resultSetObject = await _defaultMethodesRepository.AddAsync(new ResultSet
            {
                ResultSetId = Guid.NewGuid(),
                Value = promptParams.ResultSet,
                Description = promptParams.Description,
                PromptRequierements = promptParams.PromptRequirements
            });

            do
            {
                round++;

                // send message
                List<IMessage> messages = ObjectCreationForApi.CreateMessageForApi(promptParams.SystemPrompt(), promptParams.Message());

                var systemPromptObject = new SystemPrompt
                {
                    SystemPromptId = Guid.NewGuid(),
                    Value = messages[0]!.Content!,
                };

                foreach (var modelName in promptParams.ModelNames)
                {
                    bool isOpenAiModel = modelName.ToLower().Contains("gpt");

                    await LoadModelinLmStudioIfNeeded(statusHandler, isOpenAiModel, modelName);
                    IApiResult<ResponseData> apiResult = await CreateApiResult(promptParams, statusHandler, messages, isOpenAiModel, modelName);

                    var model = await _defaultMethodesRepository.ViewModelOverValueAysnc(modelName);

                    Result result = CreateResult(promptParams, messages, systemPromptObject, apiResult, model);

                    displayResult(result);

                    // set rating                    
                    var ratingReasonValue = promptParams.RatingReason();
                    var ratingValue = promptParams.Rating();

                    PromptRatingRound promptRatingRound = CreatePromptRatingRound(round, result, ratingReasonValue, ratingValue);

                    result.PromptRatingRound = promptRatingRound;
                    await SaveToDatabase(promptParams, statusHandler, messages, modelName, apiResult, result);
                }
            }
            while (promptParams.NewImprovement());
        }

        private async Task SaveToDatabase(PromptRatingUseCaseParams promptParams, StatusHandler? statusHandler, List<IMessage> messages, string modelName, IApiResult<ResponseData> apiResult, Result result)
        {
            await HandleStatus(statusHandler, $"Saving dependencies for '{modelName}'...", async () =>
            {
                await SaveDependencies.SaveDependenciesFromResult(
                    _defaultMethodesRepository,
                    messages[0]?.Content ?? string.Empty,
                    promptParams.ResultSet,
                    apiResult,
                    result,
                    apiResult?.Data?.Object ?? string.Empty,
                    apiResult?.Data?.Choices?.FirstOrDefault()?.FinishReason ?? string.Empty
                );

                await _defaultMethodesRepository.AddAsync(result);
            });
        }

        private async Task<IApiResult<ResponseData>> CreateApiResult(
            PromptRatingUseCaseParams promptParams,
            StatusHandler? statusHandler,
            List<IMessage> messages,
            bool openAiModel,
            string modelName)
        {
            IApiResult<ResponseData> apiResult = await HandleStatus(
                statusHandler,
                $"Sending chat request for '{modelName}'...",
                async () =>
                {
                    JsonElement? json = null;
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(promptParams?.ResponseFormat))
                            json = JsonDocument.Parse(promptParams.ResponseFormat).RootElement;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"JSON Parsing Error: {ex.Message}");
                        throw;
                    }
                    // Configure the RequestData object based on whether it's an OpenAI model
                    var requestData = new RequestData
                    {
                        Model = modelName,
                        Messages = messages,
                        Temperature = 0, // Assuming temperature is 0 as in the original method
                        MaxTokens = openAiModel && promptParams.MaxTokens == -1 ? null : promptParams.MaxTokens,
                        Source = openAiModel
                    ? Configuration.ApiSourceChatGpt
                    : Configuration.ApiSourceLmStudio,
                        EnvironmentTokenName = openAiModel ? Configuration.EnvironmentTokenName : null,
                        ResponseFormat = json,
                    };

                    return await _apiRequester.SendChatRequestAsync(requestData);
                });

            return apiResult;
        }

        private async Task LoadModelinLmStudioIfNeeded(StatusHandler? statusHandler, bool OpenAiModel, string modelName)
        {
            if (!OpenAiModel)
                await HandleStatus(statusHandler, $"Loading model '{modelName}'...", async () =>
                {
                    _loadUnloadLms.Load(modelName);
                });
        }

        private static PromptRatingRound CreatePromptRatingRound(int round, Result result, string ratingReasonValue, int ratingValue)
        {
            return new PromptRatingRound
            {
                PromptRatingRoundId = Guid.NewGuid(),
                ResultId = result.ResultId,
                Rating = ratingValue,
                ReasenRating = ratingReasonValue,
                Round = round,
                Result = result
            };
        }

        private static Result CreateResult(PromptRatingUseCaseParams promptParams, List<IMessage> messages, SystemPrompt systemPromptObject, IApiResult<ResponseData> apiResult, Model model)
        {
            return new Result
            {
                ResultId = Guid.NewGuid(),
                RequestId = apiResult?.Data?.Id,
                Asked = messages[1].Content,
                Message = apiResult?.Data?.Choices?.FirstOrDefault()?.Message?.Content,
                ResponseFormat = promptParams.ResponseFormat,
                Temperature = 0,
                MaxTokens = promptParams.MaxTokens,
                PromptTokens = apiResult?.Data?.Usage?.PromptTokens ?? 0,
                CompletionTokens = apiResult?.Data?.Usage?.CompletionTokens ?? 0,
                TotalTokens = apiResult?.Data?.Usage?.TotalTokens ?? 0,
                RequestCreated = DateTimeOffset.FromUnixTimeSeconds(apiResult?.Data?.Created ?? 0).UtcDateTime,
                RequestStart = apiResult!.RequestStart,
                RequestEnd = apiResult.RequestEnd,
                SystemPrompt = systemPromptObject,
                Model = model,
            };
        }

        private async Task<T> HandleStatus<T>(StatusHandler? statusHandler, string statusMessage, Func<Task<T>> action)
        {
            if (statusHandler != null)
            {
                T result = default!;
                statusHandler(statusMessage, () =>
                {
                    result = action().Result;
                });
                return result;
            }
            return await action();
        }

        private async Task HandleStatus(StatusHandler? statusHandler, string statusMessage, Func<Task> action)
        {
            if (statusHandler != null)
            {
                statusHandler(statusMessage, () =>
                {
                    action().Wait();
                });
            }
            else
            {
                await action();
            }
        }
    }
}
