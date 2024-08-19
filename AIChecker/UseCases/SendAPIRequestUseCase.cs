using de.devcodemonkey.AIChecker.CoreBusiness.DbModels;
using de.devcodemonkey.AIChecker.CoreBusiness.Interfaces;
using de.devcodemonkey.AIChecker.CoreBusiness.Models;
using de.devcodemonkey.AIChecker.DataSource.APIRequester;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace de.devcodemonkey.AIChecker.UseCases
{
    public class SendAPIRequestUseCase
    {
        private readonly APIRequester _apiRequester;

        public SendAPIRequestUseCase(APIRequester apiRequester) => _apiRequester = apiRequester;

        public async Task<IApiResult<ResponseData>> ExecuteAsync(
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

            return await _apiRequester.SendPostRequest<RequestData, ResponseData>(source, requestData, environmentTokenName);
        }
    }
}
