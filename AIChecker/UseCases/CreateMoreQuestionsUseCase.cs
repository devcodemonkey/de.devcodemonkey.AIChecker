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
    public class CreateMoreQuestionsUseCase : ICreateMoreQuestionsUseCase
    {
        private readonly IAPIRequester _apiRequester;
        private readonly IDefaultMethodesRepository _defaultMethodesRepository;

        public delegate void StatusHandler(string statusMessage, Action action);

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

            var resultSetObject = await _defaultMethodesRepository.AddAsync(new ResultSet
            {
                ResultSetId = Guid.NewGuid(),
                Value = moreQuestionsUseCaseParams.ResultSet,
            });

            IEnumerable<Question> questions = await _defaultMethodesRepository.ViewQuestionAnswerByCategoryAsync(moreQuestionsUseCaseParams.Category);

            IEnumerable<string> questionAnswer = ConcatQuestionAnswer(questions);

            SystemPrompt systemPromptObject = ObjectCreationForApi.CreateSystemPrompt(moreQuestionsUseCaseParams.SystemPrompt);

            var model = await _defaultMethodesRepository.ViewModelOverValueAysnc(moreQuestionsUseCaseParams.Model);

            for (int i = 0; i < questions.Count(); i++)
            {
                List<IMessage> messages = ObjectCreationForApi.CreateMessageForApi(moreQuestionsUseCaseParams.SystemPrompt, questionAnswer.ElementAt(i));

                var apiResult = await SendChatRequestAsync(moreQuestionsUseCaseParams, messages);

                var resultDb = ObjectCreationForApi.CreateResult(
                    questionAnswer.ElementAt(i),
                    moreQuestionsUseCaseParams.ResponseFormat,
                    moreQuestionsUseCaseParams.MaxTokens,
                    systemPromptObject,
                    apiResult,
                    model);

                resultDb.AnswerId = questions.ElementAt(i).AnswerId;

                await SaveDependencies.SaveDependenciesFromResult(
                    _defaultMethodesRepository,
                    moreQuestionsUseCaseParams.SystemPrompt,
                    moreQuestionsUseCaseParams.ResultSet,
                    apiResult,
                    resultDb,
                    apiResult.Data!.Object!,
                    apiResult.Data.Choices?.FirstOrDefault()?.FinishReason!);

                await _defaultMethodesRepository.AddAsync(resultDb);
            }

        }

        private async Task<IApiResult<ResponseData>> SendChatRequestAsync(MoreQuestionsUseCaseParams moreQuestionsUseCaseParams, List<IMessage> messages)
        {
            var requestData = new RequestData
            {
                Messages = messages,
                Model = moreQuestionsUseCaseParams.Model,
                MaxTokens = moreQuestionsUseCaseParams.MaxTokens,
                Temperature = moreQuestionsUseCaseParams.Temperature,
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
    }
}
