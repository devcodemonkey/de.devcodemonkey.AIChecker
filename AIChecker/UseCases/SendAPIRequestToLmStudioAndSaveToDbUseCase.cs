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

        public async Task ExecuteAsync(List<string> UserMessages,
            string systemPromt,
            string resultSet,
            double temperture = 0.7)
        {

            // create messages with user messages and the same system promt
            foreach (var userMessage in UserMessages)
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


                var apiResult = await _apiRequester.SendChatRequestAsync(messages, maxTokens: -1, temperature: temperture);

                Result result = new Result
                {
                    ResultId = Guid.NewGuid(),
                    Message = apiResult.Data.Choices[0].Message.Content,
                    Asked = messages[0].Content,
                    Temperture = temperture,
                    RequestStart = apiResult.RequestStart,
                    RequestEnd = apiResult.RequestEnd
                };

                // check if model exists in db
                var modelExists = await _defaultMethodesRepository.ViewOverValue<Model>(apiResult.Data.Model);
                if (modelExists == null)
                {
                    result.Model = new Model
                    {
                        ModelId = Guid.NewGuid(),
                        Value = apiResult.Data.Model
                    };
                }
                else
                    result.ModelId = modelExists.ModelId;

                // check if resultSet exists in db
                var resultSetExists = await _defaultMethodesRepository.ViewOverValue<ResultSet>(resultSet);
                if (resultSetExists == null)
                {
                    result.ResultSet = new ResultSet
                    {
                        ResultSetId = Guid.NewGuid(),
                        Value = resultSet
                    };
                }
                else
                    result.ResultSetId = resultSetExists.ResultSetId;

                // check if systemPromt exists in db
                var systemPromtExists = await _defaultMethodesRepository.ViewOverValue<SystemPromt>(systemPromt);
                if (systemPromtExists == null)
                {
                    result.SystemPromt = new SystemPromt
                    {
                        SystemPromtId = Guid.NewGuid(),
                        Value = systemPromt
                    };
                }
                else
                    result.SystemPromtId = systemPromtExists.SystemPromtId;

                await _defaultMethodesRepository.AddAsync(result);

            }
        }
    }
}
