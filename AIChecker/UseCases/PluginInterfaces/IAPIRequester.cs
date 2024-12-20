﻿using de.devcodemonkey.AIChecker.CoreBusiness.Interfaces;
using de.devcodemonkey.AIChecker.CoreBusiness.Models;
using de.devcodemonkey.AIChecker.DataSource.APIRequester.Interfaces;
using de.devcodemonkey.AIChecker.DataSource.APIRequester.Models;

namespace de.devcodemonkey.AIChecker.UseCases.PluginInterfaces
{
    public interface IAPIRequester
    {
        Task<IApiResult<ResponseData>> SendChatRequestAsync(RequestData requestData);
        Task<IApiResult<ResponseData>> SendChatRequestOldAsync(List<IMessage> messages, string model = "nothing set", string source = "http://localhost:1234/v1/chat/completions", double temperature = 0, int? maxTokens = null, bool stream = false, string? environmentTokenName = null, TimeSpan? requestTimeout = null);
        Task<ApiResult<TResponse>> SendPostRequest<TRequest, TResponse>(string source, TRequest request, string? environmentBearerTokenName = null, TimeSpan? requestTimeout = null) where TRequest : class;
    }
}