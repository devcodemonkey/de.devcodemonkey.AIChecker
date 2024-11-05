using de.devcodemonkey.AIChecker.CoreBusiness.DbModels;
using de.devcodemonkey.AIChecker.CoreBusiness.Interfaces;
using de.devcodemonkey.AIChecker.CoreBusiness.Models;
using de.devcodemonkey.AIChecker.DataSource.APIRequester.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace de.devcodemonkey.AIChecker.UseCases
{
    public class ObjectCreationForApi
    {
        public static List<IMessage> CreateMessageForApi(string systemPrompt, string message)
        {
            return new List<IMessage> {
                new Message
                {
                    Role = "user",
                    Content = message
                },
                new Message
                {
                    Role = "system",
                    Content = systemPrompt
                }
            };
        }

        public static Result CreateResult(string asked, string responseFormat, int? maxTokens, SystemPrompt systemPromptObject, IApiResult<ResponseData> apiResult, Model model)
        {
            return new Result
            {
                ResultId = Guid.NewGuid(),
                RequestId = apiResult?.Data?.Id,
                Asked = asked,
                Message = apiResult?.Data?.Choices?.FirstOrDefault()?.Message?.Content,
                ResponseFormat = responseFormat,
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
        }
    }
}
