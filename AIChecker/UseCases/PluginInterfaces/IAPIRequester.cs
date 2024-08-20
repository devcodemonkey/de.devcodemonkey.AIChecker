using de.devcodemonkey.AIChecker.CoreBusiness.Interfaces;
using de.devcodemonkey.AIChecker.CoreBusiness.Models;
using de.devcodemonkey.AIChecker.DataSource.APIRequester.Interfaces;
using de.devcodemonkey.AIChecker.DataSource.APIRequester.Models;

namespace de.devcodemonkey.AIChecker.UseCases.PluginInterfaces
{
    public interface IAPIRequester
    {
        Task<IApiResult<ResponseData>> SendChatRequestAsync(List<IMessage> messages, string model = "nothing set", string source = "http://localhost:1234/v1/chat/completions", double temperature = 0, int? maxTokens = null, bool stream = false, string? environmentTokenName = null);
        Task<ApiResult<TResponse>> SendPostRequest<TRequest, TResponse>(string source, TRequest request, string? environmentBearerTokenName = null) where TRequest : class;
    }
}