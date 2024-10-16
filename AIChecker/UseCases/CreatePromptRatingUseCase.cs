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

        public async Task ExecuteAsync(string[] modelNames, int maxTokens, string resultSet, string promptRequierements, Func<string> systemPrompt,
            Func<string> message, Func<int> ranking, Func<bool> newImprovement, Action<Result> DisplayResult,
            StatusHandler? statusHandler = null)
        {
            int round = 0;

            var resultSetObject = await _defaultMethodesRepository.AddAsync(new ResultSet
            {
                ResultSetId = Guid.NewGuid(),
                Value = resultSet,
                PromptRequierements = promptRequierements
            });


            do
            {
                round++;

                // send message
                List<IMessage> messages = new();
                messages.Add(new Message
                {
                    Role = "system",
                    Content = systemPrompt()
                });
                messages.Add(new Message
                {
                    Role = "user",
                    Content = message()
                });                

                var systemPromptObject = new SystemPrompt
                {
                    SystemPromptId = Guid.NewGuid(),
                    Value = messages[1]!.Content!,
                };

                foreach (var modelName in modelNames)
                {
                    // change model
                    await HandleStatus(statusHandler, $"Loading model '{modelName}'...", async () =>
                    {
                        _loadUnloadLms.Load(modelName);
                    });

                    IApiResult<ResponseData> apiResult = await HandleStatus(statusHandler, $"Sending chat request for '{modelName}'...", async () =>
                    {
                        return await _apiRequester.SendChatRequestAsync(messages, maxTokens: maxTokens);
                    });

                    var model = await _defaultMethodesRepository.ViewModelOverValueAysnc(modelName);

                    // save result                    
                    var result = new Result
                    {
                        ResultId = Guid.NewGuid(),
                        Asked = messages[0].Content,
                        Message = apiResult?.Data?.Choices?.FirstOrDefault()?.Message?.Content,
                        Temperature = 0,
                        MaxTokens = maxTokens,
                        PromptTokens = apiResult?.Data?.Usage?.PromptTokens ?? 0,
                        CompletionTokens = apiResult?.Data?.Usage?.CompletionTokens ?? 0,
                        TotalTokens = apiResult?.Data?.Usage?.TotalTokens ?? 0,
                        RequestCreated = DateTimeOffset.FromUnixTimeSeconds(apiResult?.Data?.Created ?? 0).UtcDateTime,
                        RequestStart = apiResult!.RequestStart,
                        RequestEnd = apiResult.RequestEnd,
                        SystemPrompt = systemPromptObject,
                        Model = model,
                    };


                    // return result to show in UI
                    DisplayResult(result);

                    // set ranking
                    var rankingValue = ranking();

                    await HandleStatus(statusHandler, $"Saving dependencies for '{modelName}'...", async () =>
                    {
                        await SaveDependencies.SaveDependenciesFromResult(
                            _defaultMethodesRepository,
                            messages[1]!.Content!,
                            resultSet,
                            apiResult,
                            result,
                            apiResult!.Data!.Object!,
                            apiResult?.Data?.Choices?.FirstOrDefault()?.FinishReason ?? string.Empty
                        );

                        PromptRatingRound promptRatingRound = new PromptRatingRound
                        {
                            PromptRatingRoundId = Guid.NewGuid(),
                            ResultId = result.ResultId,
                            Rating = rankingValue,
                            Round = round,
                            Result = result
                        };


                        result.PromptRatingRound = promptRatingRound;


                        await _defaultMethodesRepository.AddAsync(result);
                    });
                }
            }
            while (newImprovement());
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
