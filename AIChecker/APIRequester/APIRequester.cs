using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace de.devcodemonkey.AIChecker.DataSource.APIRequester
{
    public class APIRequester
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

                var response = await client.PostAsync(source, content);

                var apiResult = new ApiResult<TResponse>
                {
                    StatusCode = response.StatusCode
                };

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    apiResult.Data = JsonSerializer.Deserialize<TResponse>(responseContent, options: jsonSerializerOptions);
                }

                return apiResult;
            }
        }
    }
}
