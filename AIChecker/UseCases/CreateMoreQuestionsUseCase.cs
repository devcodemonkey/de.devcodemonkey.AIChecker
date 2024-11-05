using de.devcodemonkey.AIChecker.CoreBusiness.DbModels;
using de.devcodemonkey.AIChecker.CoreBusiness.Interfaces;
using de.devcodemonkey.AIChecker.CoreBusiness.Models;
using de.devcodemonkey.AIChecker.DataSource.APIRequester.Interfaces;
using de.devcodemonkey.AIChecker.UseCases.Interfaces;
using de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;
using System.Net;
using System.Text.Json;

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

        public async Task ExecuteAsync(MoreQuestionsUseCaseParams moreQuestionsUseCaseParams)
        {
            if (moreQuestionsUseCaseParams.Category == null)
                throw new ArgumentNullException(nameof(moreQuestionsUseCaseParams.Category));
            if (!moreQuestionsUseCaseParams.Model.Contains("gpt"))
                throw new ArgumentException("Only OpenAi Models are supported. Model must contain 'gpt' in the name", nameof(moreQuestionsUseCaseParams.Model));

            List<IMessage> messages = CreateMessageForApi(moreQuestionsUseCaseParams.SystemPrompt, moreQuestionsUseCaseParams.Message);

            IEnumerable<Question> questions = await _defaultMethodesRepository.ViewQuestionAnswerByCategoryAsync(moreQuestionsUseCaseParams.Category);

            IEnumerable<string> questionAnswer = ConcatQuestionAnswer(questions);

            for (int i = 0; i < questions.Count(); i++)
            {
                var apiResult = await SendChatRequestAsync(moreQuestionsUseCaseParams, messages);

            }

        }

        private async Task<IApiResult<ResponseData>> SendChatRequestAsync(MoreQuestionsUseCaseParams moreQuestionsUseCaseParams, List<IMessage> messages)
        {
            var requestData = new RequestData
            {
                Messages = messages,
                Model = moreQuestionsUseCaseParams.Model,
                MaxTokens = moreQuestionsUseCaseParams.MaxTokens,
                Temperature = 0,
                Stream = false,
                EnvironmentTokenName = Configuration.EnvironmentTokenName,
                Source = Configuration.ApiSourceChatGpt,
                RequestTimeout = null
            };
            var apiResponse = await _apiRequester.SendChatRequestAsync(requestData);
            if (apiResponse.StatusCode != HttpStatusCode.OK)
                throw new Exception($"Request failed with status code {apiResponse.StatusCode}, sended Request: {JsonSerializer.Serialize(requestData)}");
            return apiResponse;
        }

        private IEnumerable<string> ConcatQuestionAnswer(IEnumerable<Question> questions)
        {
            foreach (var question in questions)
            {
                yield return $"Frage:\n\"{question.Value}\"\nAntwort:\"{question.Answer.Value}";
            }
        }

        private List<IMessage> CreateMessageForApi(string systemPrompt, string message)
        {
            return new List<IMessage> {
                new Message
                {
                    Role = "user",
                    Content = message
                },
                new Message
                {
                    Role = "system",
                    Content = systemPrompt
                }
            };
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
                List<IMessage> messages = CreateMessageForApi(systemPrompt, answer);

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

        private static List<IMessage> CreateMessageForApi(string systemPrompt, Answer answer)
        {
            return new List<IMessage>() {
                    new Message
                    {
                        Role = "user",
                        Content = answer.Value
                    },
                    new Message
                    {
                        Role = "system",
                        Content = systemPrompt
                    }
                };
        }
    }
}
