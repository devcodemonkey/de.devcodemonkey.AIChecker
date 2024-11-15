using de.devcodemonkey.AIChecker.CoreBusiness.DbModels;
using de.devcodemonkey.AIChecker.CoreBusiness.Models;
using de.devcodemonkey.AIChecker.UseCases.Interfaces;
using de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;

namespace de.devcodemonkey.AIChecker.UseCases
{
    public class SendQuestionsToLmsUseCase : ISendQuestionsToLmsUseCase
    {
        private readonly IDefaultMethodesRepository _defaultMethodesRepository;
        private readonly ISendAndSaveApiRequestUseCase _sendAndSaveApiRequestUseCase;

        public SendQuestionsToLmsUseCase(IDefaultMethodesRepository defaultMethodesRepository, ISendAndSaveApiRequestUseCase sendAndSaveApiRequestUseCase)
        {
            _defaultMethodesRepository = defaultMethodesRepository;
            _sendAndSaveApiRequestUseCase = sendAndSaveApiRequestUseCase;
        }

        public async Task ExecuteAsync(SendToLmsParams sendToLmsParams)
        {
            string? questionCategory = sendToLmsParams.QuestionCategory;
            if (questionCategory == null)
                throw new ArgumentNullException(nameof(questionCategory));

            var questionsCategoryId = await _defaultMethodesRepository.ViewOverValue<QuestionCategory>(questionCategory);

            var questions = await _defaultMethodesRepository.ViewQuestionAnswerByCategoryAsync(questionsCategoryId.Value);

            var answers = await _defaultMethodesRepository.GetAllEntitiesAsync<Answer>();

            foreach (var question in questions)
            {
                foreach (var answer in answers)
                {
                    sendToLmsParams.UserMessage = ConcatQuestionAnswer(question.Value, answer.Value);
                    sendToLmsParams.QuestionId = question.QuestionId;
                    sendToLmsParams.AnswerId = answer.AnswerId;
                    await _sendAndSaveApiRequestUseCase.ExecuteAsync(sendToLmsParams);
                }                
            }
        }

        private string ConcatQuestionAnswer(string question, string answer)
            => $"Frage:\n\"{question}\"\nAntwort:\n\"{answer}\"";
    }
}
