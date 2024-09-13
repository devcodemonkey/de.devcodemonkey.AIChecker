﻿using de.devcodemonkey.AIChecker.CoreBusiness.DbModels;
using de.devcodemonkey.AIChecker.CoreBusiness.Interfaces;
using de.devcodemonkey.AIChecker.CoreBusiness.Models;
using de.devcodemonkey.AIChecker.UseCases.Interfaces;
using de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;
using System.Threading;

namespace de.devcodemonkey.AIChecker.UseCases
{
    public class SendAPIRequestToLmStudioAndSaveToDbUseCase : ISendAPIRequestToLmStudioAndSaveToDbUseCase
    {
        private readonly IAPIRequester _apiRequester;

        private readonly IDefaultMethodesRepository _defaultMethodesRepository;

        private readonly ISystemMonitor _systemMonitor;

        public SendAPIRequestToLmStudioAndSaveToDbUseCase(
            IAPIRequester apiRequester,
            IDefaultMethodesRepository defaultMethodesRepository,
            ISystemMonitor systemMonitor)
        {
            _apiRequester = apiRequester;
            _defaultMethodesRepository = defaultMethodesRepository;
            _systemMonitor = systemMonitor;
        }


        public async Task ExecuteAsync(string userMessage,
            string systemPromt,
            string resultSet,
            int requestCount = 1,
            int maxTokens = -1,
            double temperture = 0.7,
            bool saveProcessUsage = true,
            int saveInterval = 5)
        {
            await _defaultMethodesRepository.AddAsync(new ResultSet
            {
                ResultSetId = Guid.NewGuid(),
                Value = resultSet
            });

            // start monitoring
            if (saveProcessUsage)
            {
                using (var cancellationTokenSource = new CancellationTokenSource())
                {
                    // Start the monitoring task
                    var monitoringTask = _systemMonitor.MonitorPerformanceEveryXSecondsAsync(async (applicationUsages) =>
                    {
                        foreach(var item in applicationUsages)                        
                            item.SystemResourceUsageId = Guid.NewGuid();
                        
                        // Save the application usages
                        await _defaultMethodesRepository.AddAsync(applicationUsages);

                    }, saveInterval, cancellationTokenSource.Token);

                    // example process to monitor for 20 seconds
                    await SaveApiRequest(userMessage, systemPromt, resultSet, requestCount, maxTokens, temperture);

                    cancellationTokenSource.Cancel();

                    // Await the monitoring task to handle final cleanup and stop gracefully
                    await monitoringTask;
                }

            }
            else
                await SaveApiRequest(userMessage, systemPromt, resultSet, requestCount, maxTokens, temperture);

        }

        private async Task SaveApiRequest(string userMessage, string systemPromt, string resultSet, int requestCount, int maxTokens, double temperture)
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
                Content = systemPromt
            });

            for (int i = 0; i < requestCount; i++)
            {

                var apiResult = await _apiRequester.SendChatRequestAsync(messages, maxTokens: maxTokens, temperature: temperture);


                Result result = new Result
                {
                    ResultId = Guid.NewGuid(),
                    RequestId = apiResult?.Data?.Id,
                    Asked = messages[0].Content,
                    Message = apiResult?.Data?.Choices?.FirstOrDefault()?.Message?.Content,
                    Temperature = temperture,
                    MaxTokens = maxTokens,
                    PromtTokens = apiResult?.Data?.Usage?.PromptTokens ?? 0,
                    CompletionTokens = apiResult?.Data?.Usage?.CompletionTokens ?? 0,
                    TotalTokens = apiResult?.Data?.Usage?.TotalTokens ?? 0,
                    RequestCreated = DateTimeOffset.FromUnixTimeSeconds(apiResult?.Data?.Created ?? 0).UtcDateTime,
                    RequestStart = apiResult!.RequestStart,
                    RequestEnd = apiResult.RequestEnd
                };

                await SaveDependencies.SaveDependenciesFromResult(_defaultMethodesRepository,
                    systemPromt,
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
