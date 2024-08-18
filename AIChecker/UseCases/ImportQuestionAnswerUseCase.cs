using de.devcodemonkey.AIChecker.CoreBusiness.DbModels;
using de.devcodemonkey.AIChecker.CoreBusiness.Models;
using de.devcodemonkey.AIChecker.UseCases.Interfaces;
using de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task ExecuteAsnc(string filePath)
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
                        Value = questionAnswer.Answer
                    }
                };
                questions.Add(question);
            }

            await _defaultMethodesRepository.AddAsync(questions);
        }

    }
}
