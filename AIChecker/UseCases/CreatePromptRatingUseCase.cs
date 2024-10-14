using de.devcodemonkey.AIChecker.CoreBusiness.DbModels;
using de.devcodemonkey.AIChecker.CoreBusiness.Interfaces;
using de.devcodemonkey.AIChecker.CoreBusiness.Models;
using de.devcodemonkey.AIChecker.DataSource.APIRequester.Interfaces;
using de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace de.devcodemonkey.AIChecker.UseCases
{
    public class CreatePromptRatingUseCase
    {
        private readonly IDefaultMethodesRepository _defaultMethodesRepository;
        private readonly ILoadUnloadLms _loadUnloadLms;
        private readonly IAPIRequester _apiRequester;

        public CreatePromptRatingUseCase(IDefaultMethodesRepository defaultMethodesRepository, ILoadUnloadLms loadUnloadLms, IAPIRequester aPIRequester)
        {
            _defaultMethodesRepository = defaultMethodesRepository;
            _loadUnloadLms = loadUnloadLms;
            _apiRequester = aPIRequester;
        }

        public async Task ExecuteAsync(string[] modelNames, int maxTokens, string resultSet, Func<string> systemPrompt,
            Func<string> message, Func<int> ranking, Func<bool> newImprovement, Action<ResponseData> apiResponse)
        {
            do
            {
                foreach (var modelName in modelNames)
                {
                    // change model
                    _loadUnloadLms.Load(modelName);

                    // send message
                    List<IMessage> messages = new();
                    messages.Add(new Message
                    {
                        Role = "user",
                        Content = message()
                    });
                    messages.Add(new Message
                    {
                        Role = "system",
                        Content = systemPrompt()
                    });

                    IApiResult<ResponseData> apiResult = await _apiRequester.SendChatRequestAsync(messages, maxTokens: maxTokens);
                    // return result to show in UI
                    apiResponse(apiResult.Data);

                    // save result
                    var rankingValue = ranking();
                    var result = new Result
                    {
                        ResultId = Guid.NewGuid(),
                        Message = message(),
                        Asked = messages[0].Content,
                        Temperature = 0,
                        MaxTokens = maxTokens,
                        PromptTokens = apiResult?.Data?.Usage?.PromptTokens ?? 0,
                        CompletionTokens = apiResult?.Data?.Usage?.CompletionTokens ?? 0,
                        TotalTokens = apiResult?.Data?.Usage?.TotalTokens ?? 0,
                        RequestCreated = DateTimeOffset.FromUnixTimeSeconds(apiResult?.Data?.Created ?? 0).UtcDateTime,
                        RequestStart = apiResult!.RequestStart,
                        RequestEnd = apiResult.RequestEnd,
                        Rating = rankingValue
                    };
                    await SaveDependencies.SaveDependenciesFromResult(_defaultMethodesRepository,
                        systemPrompt(),
                        resultSet,
                        apiResult,
                        result,
                        apiResult!.Data!.Object!,
                        apiResult?.Data?.Choices?.FirstOrDefault()?.FinishReason ?? string.Empty
                        );
                }
            }
            while (newImprovement());
        }
    }
}
