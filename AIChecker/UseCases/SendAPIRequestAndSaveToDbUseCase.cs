using de.devcodemonkey.AIChecker.CoreBusiness.DbModels;
using de.devcodemonkey.AIChecker.CoreBusiness.Interfaces;
using de.devcodemonkey.AIChecker.CoreBusiness.Models;
using de.devcodemonkey.AIChecker.DataSource.APIRequester.Interfaces;
using de.devcodemonkey.AIChecker.UseCases.Global;
using de.devcodemonkey.AIChecker.UseCases.Interfaces;
using de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;
using System.Net;
using System.Text.Json;

namespace de.devcodemonkey.AIChecker.UseCases
{
    public class SendAPIRequestAndSaveToDbUseCase : ISendAndSaveApiRequestUseCase
    {
        private readonly IAPIRequester _apiRequester;

        private readonly IDefaultMethodesRepository _defaultMethodesRepository;

        private readonly ISystemMonitor _systemMonitor;

        public SendAPIRequestAndSaveToDbUseCase(
            IAPIRequester apiRequester,
            IDefaultMethodesRepository defaultMethodesRepository,
            ISystemMonitor systemMonitor)
        {
            _apiRequester = apiRequester;
            _defaultMethodesRepository = defaultMethodesRepository;
            _systemMonitor = systemMonitor;
        }

        public async Task ExecuteAsync(SendToLmsParams sendToLmsParams)
        {
            var resultSetObject = await _defaultMethodesRepository.ViewOverValue<ResultSet>(sendToLmsParams.ResultSet);            
            if (resultSetObject == null)
                resultSetObject = await _defaultMethodesRepository.AddAsync(new ResultSet
                {
                    ResultSetId = Guid.NewGuid(),
                    Value = sendToLmsParams.ResultSet,
                });            

            if (sendToLmsParams.SaveProcessUsage)
                await SaveProcessUsage(sendToLmsParams, resultSetObject);
            else
                await SendAndSaveApiRequest(sendToLmsParams);
        }

        private async Task SendAndSaveApiRequest(SendToLmsParams sendToLmsParams)
        {
            List<IMessage> messages = ObjectCreationForApi.CreateMessageForApi(sendToLmsParams.SystemPrompt, sendToLmsParams.UserMessage);

            for (int i = 0; i < sendToLmsParams.RequestCount; i++)
            {
                var apiResult = await SendChatRequestAsync(sendToLmsParams, messages);

                var resultDb = ObjectCreationForApi.CreateResult(
                    sendToLmsParams,
                    ObjectCreationForApi.CreateSystemPrompt(sendToLmsParams.SystemPrompt),
                    apiResult,
                    await _defaultMethodesRepository.ViewModelOverValueAysnc(sendToLmsParams.Model));

                await SaveDependencies.SaveDependenciesFromResult(_defaultMethodesRepository,
                    sendToLmsParams.SystemPrompt,
                    sendToLmsParams.ResultSet,
                    apiResult,
                    resultDb,
                    apiResult.Data!.Object!,
                    apiResult.Data.Choices?.FirstOrDefault()?.FinishReason!);

                await _defaultMethodesRepository.AddAsync(resultDb);
            }
        }

        private async Task<IApiResult<ResponseData>> SendChatRequestAsync(SendToLmsParams sendToLmsParams, List<IMessage> messages)
        {
            var json = JsonValidator.ConvertToJsonFormat(sendToLmsParams.ResponseFormat);

            var requestData = new RequestData
            {
                Messages = messages,
                MaxTokens = sendToLmsParams.MaxTokens,
                Temperature = sendToLmsParams.Temperature,
                Model = sendToLmsParams.Model,
                Source = sendToLmsParams.Source,
                EnvironmentTokenName = sendToLmsParams.EnvironmentTokenName,
                Stream = false,
                RequestTimeout = null,
                ResponseFormat = json
            };
            var apiResponse = await _apiRequester.SendChatRequestAsync(requestData);
            if (apiResponse.StatusCode != HttpStatusCode.OK)
                throw new Exception($"Request failed with status code {apiResponse.StatusCode}, sended Request: {JsonSerializer.Serialize(requestData)}");
            return apiResponse;
        }

        private async Task SaveProcessUsage(SendToLmsParams sendToLmsParams, ResultSet ResultSetObject)
        {
            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                var monitoringTask = _systemMonitor.MonitorPerformanceEveryXSecondsAsync(async (applicationUsages) =>
                {
                    foreach (var item in applicationUsages)
                        item.SystemResourceUsageId = Guid.NewGuid();
                    foreach (var item in applicationUsages)
                    {
                        item.SystemResourceUsageId = Guid.NewGuid();
                        item.ResultSetId = ResultSetObject.ResultSetId;
                    }

                    await _defaultMethodesRepository.AddAsync(applicationUsages);

                }, sendToLmsParams.SaveInterval, cancellationTokenSource.Token, writeOutput: sendToLmsParams.WriteOutput);

                await SendAndSaveApiRequest(sendToLmsParams);

                cancellationTokenSource.Cancel();

                await monitoringTask;
            }
        }

        [Obsolete("use ExecuteAsync methode with parameter SendToLmsParams")]
        public async Task ExecuteAsync(string userMessage,
            string systemPrompt,
            string resultSetValue,
            int requestCount = 1,
            int maxTokens = -1,
            double temperature = 0.7,
            bool saveProcessUsage = true,
            int saveInterval = 5,
            bool writeOutput = true,
            string? environmentTokenName = null,
            string source = "http://localhost:1234/v1/chat/completions",
            string model = "nothing set")
        {
            var resultSet = await _defaultMethodesRepository.AddAsync(new ResultSet
            {
                ResultSetId = Guid.NewGuid(),
                Value = resultSetValue
            });

            // start monitoring
            if (saveProcessUsage)
            {
                using (var cancellationTokenSource = new CancellationTokenSource())
                {
                    // Start the monitoring task
                    var monitoringTask = _systemMonitor.MonitorPerformanceEveryXSecondsAsync(async (applicationUsages) =>
                    {
                        foreach (var item in applicationUsages)
                            item.SystemResourceUsageId = Guid.NewGuid();
                        foreach (var item in applicationUsages)
                        {
                            item.SystemResourceUsageId = Guid.NewGuid();
                            item.ResultSetId = resultSet.ResultSetId;
                        }

                        // Save the application usages
                        await _defaultMethodesRepository.AddAsync(applicationUsages);

                    }, saveInterval, cancellationTokenSource.Token, writeOutput: writeOutput);

                    // example process to monitor for 20 seconds
                    await SaveApiRequest(userMessage, systemPrompt, resultSetValue, requestCount, maxTokens, temperature, environmentTokenName = null, source, model);

                    cancellationTokenSource.Cancel();

                    // Await the monitoring task to handle final cleanup and stop gracefully
                    await monitoringTask;
                }
            }
            else
                await SaveApiRequest(userMessage, systemPrompt, resultSetValue, requestCount, maxTokens, temperature, environmentTokenName, source, model);

        }

        private async Task SaveApiRequest(string userMessage, string systemPrompt, string resultSet, int requestCount, int maxTokens, double temperature, string environmentBearerTokenName, string source, string model)
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
                Content = systemPrompt
            });

            for (int i = 0; i < requestCount; i++)
            {

                var apiResult = await _apiRequester.SendChatRequestOldAsync(messages, maxTokens: maxTokens, temperature: temperature, model: model, source: source, environmentTokenName: environmentBearerTokenName);


                Result result = new Result
                {
                    ResultId = Guid.NewGuid(),
                    RequestId = apiResult?.Data?.Id,
                    Asked = messages[0].Content,
                    Message = apiResult?.Data?.Choices?.FirstOrDefault()?.Message?.Content,
                    Temperature = temperature,
                    MaxTokens = maxTokens,
                    PromptTokens = apiResult?.Data?.Usage?.PromptTokens ?? 0,
                    CompletionTokens = apiResult?.Data?.Usage?.CompletionTokens ?? 0,
                    TotalTokens = apiResult?.Data?.Usage?.TotalTokens ?? 0,
                    RequestCreated = DateTimeOffset.FromUnixTimeSeconds(apiResult?.Data?.Created ?? 0).UtcDateTime,
                    RequestStart = apiResult!.RequestStart,
                    RequestEnd = apiResult.RequestEnd
                };

                await SaveDependencies.SaveDependenciesFromResult(_defaultMethodesRepository,
                    systemPrompt,
                    resultSet,
                    apiResult,
                    result,
                    apiResult!.Data!.Object!,
                    apiResult!.Data!.Choices!.FirstOrDefault()!.FinishReason ?? string.Empty);

                await _defaultMethodesRepository.AddAsync(result);
            }
        }
    }
}
