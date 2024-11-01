using de.devcodemonkey.AIChecker.CoreBusiness.DbModels;
using de.devcodemonkey.AIChecker.CoreBusiness.Interfaces;
using de.devcodemonkey.AIChecker.CoreBusiness.Models;
using de.devcodemonkey.AIChecker.UseCases.Interfaces;
using de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;

namespace de.devcodemonkey.AIChecker.UseCases
{
    public class CreateMoreQuestionsUseCase : ICreateMoreQuestionsUseCase
    {
        private readonly IAPIRequester _apiRequester;

        private readonly IDefaultMethodesRepository _defaultMethodesRepository;

        public CreateMoreQuestionsUseCase(IAPIRequester apiRequester,
            IDefaultMethodesRepository defaultMethodesRepository)
        {
            _apiRequester = apiRequester;
            _defaultMethodesRepository = defaultMethodesRepository;
        }

        public async Task ExecuteAsync(
           string resultSet,
           string systemPrompt,
           int maxTokens = -1,
           double temperature = 0.7,
           string model = "nothing set",
           string source = "http://localhost:1234/v1/chat/completions",
           string? environmentTokenName = null)
        {
            var answers = await _defaultMethodesRepository.GetAllEntitiesAsync<Answer>();

            foreach (var answer in answers)
            {
                List<IMessage> messages = new();
                messages.Add(new Message
                {
                    Role = "user",
                    Content = answer.Value
                });
                messages.Add(new Message
                {
                    Role = "system",
                    Content = systemPrompt
                });

                var apiResult = await _apiRequester
                    .SendChatRequestOldAsync(messages,
                        maxTokens: -1,
                        temperature: temperature,
                        requestTimeout: TimeSpan.FromMinutes(10));

                Result result = new Result
                {
                    ResultId = Guid.NewGuid(),
                    AnswerId = answer.AnswerId,
                    RequestId = apiResult?.Data?.Id,
                    Asked = messages[0].Content,
                    Message = apiResult?.Data?.Choices?.FirstOrDefault()?.Message?.Content,
                    Temperature = temperature,
                    MaxTokens = maxTokens,
                    PromptTokens = apiResult?.Data?.Usage?.PromptTokens ?? 0,
                    CompletionTokens = apiResult?.Data?.Usage?.CompletionTokens ?? 0,
                    TotalTokens = apiResult?.Data?.Usage?.TotalTokens ?? 0,
                    RequestCreated = DateTimeOffset.FromUnixTimeSeconds(apiResult?.Data?.Created ?? 0).UtcDateTime,
                    RequestStart = apiResult!.RequestStart!,
                    RequestEnd = apiResult!.RequestEnd
                };

                await SaveDependencies.SaveDependenciesFromResult(_defaultMethodesRepository,
                    systemPrompt,
                    resultSet,
                    apiResult,
                    result,
                    apiResult.Data!.Object!,
                    apiResult.Data.Choices?.FirstOrDefault()?.FinishReason!);

                await _defaultMethodesRepository.AddAsync(result);
            }
        }
    }
}
