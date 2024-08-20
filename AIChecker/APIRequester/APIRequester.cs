using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using de.devcodemonkey.AIChecker.CoreBusiness.Interfaces;
using de.devcodemonkey.AIChecker.CoreBusiness.Models;
using de.devcodemonkey.AIChecker.DataSource.APIRequester.Interfaces;
using de.devcodemonkey.AIChecker.DataSource.APIRequester.Models;
using de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;


namespace de.devcodemonkey.AIChecker.DataSource.APIRequester
{
    public class APIRequester : IAPIRequester
    {
        public async Task<ApiResult<TResponse>> SendPostRequest<TRequest, TResponse>(
            string source,
            TRequest request,
            string? environmentBearerTokenName = null) where TRequest : class
        {
            using (var client = new HttpClient())
            {
                if (!string.IsNullOrEmpty(environmentBearerTokenName))
                {
                    var bearerToken = Environment.GetEnvironmentVariable(environmentBearerTokenName);
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {bearerToken}");
                }

                // Serialize the request data to JSON
                var jsonSerializerOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

                var jsonContent = JsonSerializer.Serialize(request, options: jsonSerializerOptions);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var apiResult = new ApiResult<TResponse>
                {
                    RequestStart = DateTime.Now
                };
                var response = await client.PostAsync(source, content);
                apiResult.RequestEnd = DateTime.Now;
                apiResult.StatusCode = response.StatusCode;



                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    apiResult.Data = JsonSerializer.Deserialize<TResponse>(responseContent, options: jsonSerializerOptions);
                }

                return apiResult;
            }
        }

        public async Task<IApiResult<ResponseData>> SendChatRequestAsync(
            List<IMessage> messages,
            string model = "nothing set",
            string source = "http://localhost:1234/v1/chat/completions",
            double temperature = 0,
            int? maxTokens = null,
            bool stream = false,
            string? environmentTokenName = null)
        {
            var requestData = new RequestData
            {
                Model = model,
                Messages = messages,
                Temperature = temperature,
                MaxTokens = maxTokens,
                Stream = stream
            };

            return await SendPostRequest<RequestData, ResponseData>(source, requestData, environmentTokenName);
        }
    }
}
