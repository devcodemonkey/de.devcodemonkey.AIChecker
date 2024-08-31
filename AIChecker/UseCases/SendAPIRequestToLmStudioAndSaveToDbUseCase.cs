using de.devcodemonkey.AIChecker.CoreBusiness.DbModels;
using de.devcodemonkey.AIChecker.CoreBusiness.Interfaces;
using de.devcodemonkey.AIChecker.CoreBusiness.Models;
using de.devcodemonkey.AIChecker.DataSource.APIRequester;
using de.devcodemonkey.AIChecker.DataSource.APIRequester.Interfaces;
using de.devcodemonkey.AIChecker.UseCases.Interfaces;
using de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace de.devcodemonkey.AIChecker.UseCases
{
    public class SendAPIRequestToLmStudioAndSaveToDbUseCase : ISendAPIRequestToLmStudioAndSaveToDbUseCase
    {
        private readonly IAPIRequester _apiRequester;

        private readonly IDefaultMethodesRepository _defaultMethodesRepository;

        public SendAPIRequestToLmStudioAndSaveToDbUseCase(IAPIRequester apiRequester, IDefaultMethodesRepository defaultMethodesRepository)
        {
            _apiRequester = apiRequester;
            _defaultMethodesRepository = defaultMethodesRepository;
        }


        public async Task ExecuteAsync(string userMessage,
            string systemPromt,
            string resultSet,            
            int requestCount = 1,
            int maxTokens = -1,
            double temperture = 0.7)
        {
            
            List<IMessage> messages = new();
            messages.Add(new Message
            {
                Role = "user",
                Content = userMessage
            });
            messages.Add(new Message
            {
                Role = "system",
                Content = systemPromt
            });

            for (int i = 0; i < requestCount; i++)
            {

                var apiResult = await _apiRequester.SendChatRequestAsync(messages, maxTokens: maxTokens, temperature: temperture);

                Result result = new Result
                {
                    ResultId = Guid.NewGuid(),
                    RequestId = apiResult.Data.Id,
                    Asked = messages[0].Content,
                    Message = apiResult.Data.Choices[0].Message.Content,                    
                    Temperature = temperture,
                    MaxTokens = maxTokens,
                    PromtTokens = apiResult.Data.Usage.PromptTokens,
                    CompletionTokens = apiResult.Data.Usage.CompletionTokens,
                    TotalTokens = apiResult.Data.Usage.TotalTokens,
                    RequestCreated = DateTimeOffset.FromUnixTimeSeconds(apiResult.Data.Created).UtcDateTime,
                    RequestStart = apiResult.RequestStart,
                    RequestEnd = apiResult.RequestEnd
                };

                await SaveDependencies.SaveDependenciesFromResult(_defaultMethodesRepository,
                    systemPromt,
                    resultSet,
                    apiResult,
                    result,
                    apiResult.Data.Object,
                    apiResult.Data.Choices[0].FinishReason);

                await _defaultMethodesRepository.AddAsync(result);
            }
        }
    }
}
