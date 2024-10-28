using de.devcodemonkey.AIChecker.CoreBusiness.DbModels;
using de.devcodemonkey.AIChecker.CoreBusiness.Models;
using de.devcodemonkey.AIChecker.UseCases.Interfaces;
using de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;

namespace de.devcodemonkey.AIChecker.UseCases
{
    public class ImportQuestionAnswerUseCase : IImportQuestionAnswerUseCase
    {
        private readonly IDeserializer<QuestionAnswer> _deserializer;
        private readonly IDefaultMethodesRepository _defaultMethodesRepository;

        public ImportQuestionAnswerUseCase(IDeserializer<QuestionAnswer> deserializer
            , IDefaultMethodesRepository defaultMethodesRepository)
        {
            _deserializer = deserializer;
            _defaultMethodesRepository = defaultMethodesRepository;
        }

        public async Task ExecuteAsync(string filePath, string Category)
        {
            var deserializedQuestionAnswers = await _deserializer.DeserialzeFileAsync(filePath);

            var category = new Category
            {
                CategoryId = Guid.NewGuid(),
                FileName = Path.GetFileName(filePath),
                Value = Category
            };

            var questions = deserializedQuestionAnswers.Select(questionAnswer =>
            {
                var answer = new Answer
                {
                    AnswerId = Guid.NewGuid(),
                    Value = questionAnswer.Answer!,
                    Imgs = questionAnswer.Images!.Select(img => new Img
                    {
                        ImagesId = Guid.NewGuid(),
                        Img1 = File.ReadAllBytes(Path.Combine(Path.GetDirectoryName(filePath)!, img))
                    }).ToList()
                };

                return new Question
                {
                    QuestionId = Guid.NewGuid(),
                    Value = questionAnswer.Question,
                    Answer = answer,
                    AnswerId = answer.AnswerId,
                    Category = category,
                    CategoryId = category.CategoryId
                };
            }).ToList();

            await _defaultMethodesRepository.AddRangeAsync(questions);
        }
    }
}

