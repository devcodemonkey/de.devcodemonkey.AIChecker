using de.devcodemonkey.AIChecker.CoreBusiness.Interfaces;
using de.devcodemonkey.AIChecker.CoreBusiness.Models;
using de.devcodemonkey.AIChecker.DataSource.APIRequester.Interfaces;
using de.devcodemonkey.AIChecker.DataSource.APIRequester.Models;
using de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;
using System.Net.Http.Json;


namespace de.devcodemonkey.AIChecker.DataSource.APIRequester
{
    public class APIRequester : IAPIRequester
    {
        public async Task<ApiResult<TResponse>> SendPostRequest<TRequest, TResponse>(
            string source,
            TRequest request,
            string? environmentBearerTokenName = null,
            TimeSpan? requestTimeout = null) where TRequest : class
        {
            using (var client = new HttpClient())
            {
                // Set the Authorization header if a token is provided
                if (!string.IsNullOrEmpty(environmentBearerTokenName))
                {
                    var bearerToken = Environment.GetEnvironmentVariable(environmentBearerTokenName);
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);
                }

                // Set the timeout for the request
                client.Timeout = requestTimeout ?? TimeSpan.FromSeconds(100);

                // Initialize the result object
                var apiResult = new ApiResult<TResponse>
                {
                    RequestStart = DateTime.Now
                };

                // Send the request as JSON
                var response = await client.PostAsJsonAsync(source, request);
                apiResult.RequestEnd = DateTime.Now;
                apiResult.StatusCode = response.StatusCode;

                // Handle the response if successful
                if (response.IsSuccessStatusCode)
                {
                    apiResult.Data = await response.Content.ReadFromJsonAsync<TResponse>() ?? default!;
                }

                return apiResult;
            }
        }

        [Obsolete("Use SendChatRequestAsync instead.")]
        public async Task<IApiResult<ResponseData>> SendChatRequestOldAsync(
            List<IMessage> messages,
            string model = "nothing set",
            string source = "http://localhost:1234/v1/chat/completions",
            double temperature = 0,
            int? maxTokens = null,
            bool stream = false,
            string? environmentTokenName = null,
            TimeSpan? requestTimeout = null)
        {
            var requestData = new RequestData
            {
                Model = model,
                Messages = messages,
                Temperature = temperature,
                MaxTokens = maxTokens,
                Stream = stream
            };

            return await SendPostRequest<RequestData, ResponseData>(source, requestData, environmentTokenName, requestTimeout: requestTimeout);
        }

        public async Task<IApiResult<ResponseData>> SendChatRequestAsync(RequestData requestData)
        {
            return await SendPostRequest<RequestData, ResponseData>(
                requestData.Source,
                requestData,
                requestData.EnvironmentTokenName,
                requestTimeout: requestData.RequestTimeout
            );
        }
    }
}
