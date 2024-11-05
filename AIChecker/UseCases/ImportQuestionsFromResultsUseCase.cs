using de.devcodemonkey.AIChecker.CoreBusiness.DbModels;
using de.devcodemonkey.AIChecker.UseCases.Interfaces;
using de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace de.devcodemonkey.AIChecker.UseCases
{
    public class ImportQuestionsFromResultsUseCase : IImportQuestionsFromResultsUseCase
    {
        private readonly IDefaultMethodesRepository _defaultMethodesRepository;

        public ImportQuestionsFromResultsUseCase(IDefaultMethodesRepository defaultMethodesRepository)
            => _defaultMethodesRepository = defaultMethodesRepository;

        public async Task ExecuteAsync(string resultSet, string category)
        {
            IEnumerable<Result> results = await _defaultMethodesRepository.ViewResultsOfResultSetAsync(
                    await _defaultMethodesRepository.GetResultSetIdByValueAsync(resultSet));

            var questions = new List<Question>();

            var questionCategory = new QuestionCategory
            {
                QuestionCategoryId = Guid.NewGuid(),
                Value = category
            };

            List<string> notValidResults = new List<string>();

            foreach (var result in results)
            {
                if (TryParseJson<QuestionDataJson>(result.Message!, out QuestionDataJson? questionData))
                {
                    foreach (var question in questionData!.Questions)
                    {
                        questions.Add(new Question
                        {
                            QuestionId = Guid.NewGuid(),
                            AnswerId = result.AnswerId,
                            Value = question.QuestionText,
                            Category = questionCategory
                        });
                    }
                }
                else
                    notValidResults.Add(result.Message!);
            }

            if (notValidResults.Count() == 0)
            {
                await _defaultMethodesRepository.AddAsync(questionCategory);
                await _defaultMethodesRepository.AddRangeAsync(questions);
            }
            else
                throw new Exception($"Nothing was imported. Some results could not be parsed: {string.Join(", ", notValidResults)}");
        }

        public bool TryParseJson<T>(string json, out T? result)
        {
            try
            {
                result = JsonSerializer.Deserialize<T>(json);
                return result != null;
            }
            catch (JsonException)
            {
                // JSON was invalid
                result = default;
                return false;
            }
        }

    }

    public class QuestionDataJson
    {
        [JsonPropertyName("questions")]
        public List<QuestionJson> Questions { get; set; }
    }

    public class QuestionJson
    {
        [JsonPropertyName("Question")]
        public string QuestionText { get; set; }
    }
}