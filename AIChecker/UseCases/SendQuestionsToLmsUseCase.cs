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

            foreach (var question in questions)
            {
                sendToLmsParams.UserMessage = question.Value;
                await _sendAndSaveApiRequestUseCase.ExecuteAsync(sendToLmsParams);
            }
        }
    }
}
