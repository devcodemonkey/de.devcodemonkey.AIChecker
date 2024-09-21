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

        public async Task ExecuteAsync(string filePath)
        {
            var deserializedQuestionAnswers = await _deserializer.DeserialzeFileAsync(filePath);

            var questions = new List<Question>();

            foreach (var questionAnswer in deserializedQuestionAnswers)
            {
                var question = new Question
                {
                    QuestionId = Guid.NewGuid(),
                    Value = questionAnswer.Question,
                    Answer = new Answer
                    {
                        AnswerId = Guid.NewGuid(),
                        Value = questionAnswer.Answer!,
                        Imgs = new List<Img>()
                    }
                };
                foreach (var img in questionAnswer!.Images!)
                {
                    question.Answer.Imgs.Add(new Img
                    {
                        ImagesId = Guid.NewGuid(),
                        Img1 = File.ReadAllBytes(Path.Combine(Path.GetDirectoryName(filePath)!, img))
                    });
                }
                questions.Add(question);
            }

            await _defaultMethodesRepository.AddAsync(questions);
        }

    }
}
