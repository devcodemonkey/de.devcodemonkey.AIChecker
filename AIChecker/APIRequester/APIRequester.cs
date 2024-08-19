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
        public async Task<ApiResult<TResponse>> SendPostRequest<TRequest, TResponse>(string source, TRequest request) where TRequest : class
        {
            using (var client = new HttpClient())
            {
                // Serialize the request data to JSON
                var jsonContent = JsonSerializer.Serialize(request, options: new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(source, content);

                var apiResult = new ApiResult<TResponse>
                {
                    StatusCode = response.StatusCode
                };

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    apiResult.Data = JsonSerializer.Deserialize<TResponse>(responseContent);
                }

                return apiResult;
            }
        }
    }
}
