using de.devcodemonkey.AIChecker.CoreBusiness.DbModels;
using de.devcodemonkey.AIChecker.CoreBusiness.Interfaces;
using de.devcodemonkey.AIChecker.CoreBusiness.Models;
using de.devcodemonkey.AIChecker.DataSource.APIRequester.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace de.devcodemonkey.AIChecker.UseCases.Global
{
    public class ObjectCreationForApi
    {
        public static List<IMessage> CreateMessageForApi(string systemPrompt, string message)
        {
            return new List<IMessage> {                
                new Message
                {
                    Role = "system",
                    Content = systemPrompt
                },
                new Message
                {
                    Role = "user",
                    Content = message
                },
            };
        }

        public static Result CreateResult(SendToLmsParams sendToLmsParams, SystemPrompt systemPromptObject, IApiResult<ResponseData> apiResult, Model model)
        {
            return new Result
            {
                ResultId = Guid.NewGuid(),
                RequestId = apiResult?.Data?.Id,
                Asked = sendToLmsParams.UserMessage,
                Message = apiResult?.Data?.Choices?.FirstOrDefault()?.Message?.Content,
                ResponseFormat = sendToLmsParams.ResponseFormat,
                Temperature = 0,
                MaxTokens = sendToLmsParams.MaxTokens,
                PromptTokens = apiResult?.Data?.Usage?.PromptTokens ?? 0,
                CompletionTokens = apiResult?.Data?.Usage?.CompletionTokens ?? 0,
                TotalTokens = apiResult?.Data?.Usage?.TotalTokens ?? 0,
                RequestCreated = DateTimeOffset.FromUnixTimeSeconds(apiResult?.Data?.Created ?? 0).UtcDateTime,
                RequestStart = apiResult!.RequestStart,
                RequestEnd = apiResult.RequestEnd,
                SystemPrompt = systemPromptObject,
                Model = model,
                AnswerId = sendToLmsParams.AnswerId,
                QuestionsId = sendToLmsParams.QuestionId,

                PromptCachedTokens = apiResult?.Data?.Usage?.PromptTokensDetails?.CachedTokens,
                PromptAudioTokens = apiResult?.Data?.Usage?.PromptTokensDetails?.AudioTokens,
                CompletionReasoningTokens = apiResult?.Data?.Usage?.CompletionTokensDetails?.ReasoningTokens,
                CompletionAudioTokens = apiResult?.Data?.Usage?.CompletionTokensDetails?.AudioTokens,
                CompletionAcceptedPredictionTokens = apiResult?.Data?.Usage?.CompletionTokensDetails?.AcceptedPredictionTokens,
                CompletionsRejectedPredictionTokens = apiResult?.Data?.Usage?.CompletionTokensDetails?.RejectedPredictionTokens,

                SystemFingerprint = apiResult?.Data?.SystemFingerprint,
            };
        }

        public static Result CreateResult(string asked, string? responseFormat, int? maxTokens, SystemPrompt systemPromptObject, IApiResult<ResponseData> apiResult, Model model)
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

        public static SystemPrompt CreateSystemPrompt(string systemPrompt)
        {
            return new SystemPrompt
            {
                SystemPromptId = Guid.NewGuid(),
                Value = systemPrompt
            };
        }
    }
}
