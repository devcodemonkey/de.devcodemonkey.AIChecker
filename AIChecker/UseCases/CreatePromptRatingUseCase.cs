using de.devcodemonkey.AIChecker.CoreBusiness.DbModels;
using de.devcodemonkey.AIChecker.CoreBusiness.Interfaces;
using de.devcodemonkey.AIChecker.CoreBusiness.Models;
using de.devcodemonkey.AIChecker.DataSource.APIRequester.Interfaces;
using de.devcodemonkey.AIChecker.UseCases.Interfaces;
using de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;

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
                List<IMessage> messages = CreateMessageForApi(promptParams);

                var systemPromptObject = new SystemPrompt
                {
                    SystemPromptId = Guid.NewGuid(),
                    Value = messages[0]!.Content!,
                };

                bool OpenAiModel = false;
                foreach (var modelName in promptParams.ModelNames)
                {
                    bool isOpenAiModel = modelName.ToLower().Contains("gpt");

                    await LoadModelinLmStudioIfNeeded(statusHandler, OpenAiModel, modelName);
                    IApiResult<ResponseData> apiResult = await CreateApiResult(promptParams, statusHandler, messages, OpenAiModel, modelName);

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
                    messages[0]!.Content!,
                    promptParams.ResultSet,
                    apiResult,
                    result,
                    apiResult!.Data!.Object!,
                    apiResult?.Data?.Choices?.FirstOrDefault()?.FinishReason ?? string.Empty
                );

                await _defaultMethodesRepository.AddAsync(result);
            });
        }

        private async Task<IApiResult<ResponseData>> CreateApiResult(PromptRatingUseCaseParams promptParams, StatusHandler? statusHandler, List<IMessage> messages, bool OpenAiModel, string modelName)
        {
            IApiResult<ResponseData> apiResult = await HandleStatus(statusHandler, $"Sending chat request for '{modelName}'...", async () =>
            {
                // use openai api, if it's a openai model
                if (!OpenAiModel)
                    return await _apiRequester.SendChatRequestAsync(messages, maxTokens: promptParams.MaxTokens);
                else
                {
                    if (promptParams.MaxTokens == -1)
                        return await _apiRequester.SendChatRequestAsync(messages, model: modelName, temperature: 0,
                            source: "https://api.openai.com/v1/chat/completions", environmentTokenName: "OPEN_AI_TOKEN");
                    else
                        return await _apiRequester.SendChatRequestAsync(messages, model: modelName, maxTokens: promptParams.MaxTokens, temperature: 0,
                        source: "https://api.openai.com/v1/chat/completions", environmentTokenName: "OPEN_AI_TOKEN");
                }
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

        private static List<IMessage> CreateMessageForApi(PromptRatingUseCaseParams promptParams)
        {
            return [
                                new Message
                    {
                        Role = "system",
                        Content = promptParams.SystemPrompt()
                    },
                    new Message
                    {
                        Role = "user",
                        Content = promptParams.Message()
                    },
                ];
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
