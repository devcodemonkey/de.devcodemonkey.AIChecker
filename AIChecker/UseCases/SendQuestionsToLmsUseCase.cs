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

        public async Task ExecuteAsync(SendToLmsParams sendToLmsParams, Action<LoadingProgressQuestions>? progressAction = null)
        {
            string? questionCategory = sendToLmsParams.QuestionCategory;
            if (questionCategory == null)
                throw new ArgumentNullException(nameof(questionCategory));

            var startTimestamp = DateTime.Now;
            var questionsCategoryId = await _defaultMethodesRepository.ViewOverValue<QuestionCategory>(questionCategory);

            var questions = await _defaultMethodesRepository.ViewQuestionAnswerByCategoryAsync(questionsCategoryId.Value);
            if (sendToLmsParams.QuestionsCorrect)
                questions = questions.Where(q => q.Correct == true);

            var answers = await _defaultMethodesRepository.GetAllEntitiesAsync<Answer>();

            LoadingProgressQuestions loadingProgress = new LoadingProgressQuestions();

            loadingProgress.QuestionsCount = questions.Count();
            loadingProgress.AnswersCount = answers.Count();

            loadingProgress.QuestionsCounter = 0;
            loadingProgress.AnswersCounter = 0;
            foreach (var question in questions)
            {
                loadingProgress.QuestionsCounter++;
                loadingProgress.AnswersCounter = 0;
                foreach (var answer in answers)
                {
                    sendToLmsParams.UserMessage = ConcatQuestionAnswer(question.Value, answer.Value);
                    sendToLmsParams.QuestionId = question.QuestionId;
                    sendToLmsParams.AnswerId = answer.AnswerId;
                    await _sendAndSaveApiRequestUseCase.ExecuteAsync(sendToLmsParams);

                    loadingProgress.AnswersCounter++;
                    loadingProgress.TotalCounter++;
                    loadingProgress.RunningTime = DateTime.Now - startTimestamp;                    
                    loadingProgress.CalulationTime = TimeSpan.FromTicks(loadingProgress.RunningTime.Ticks / loadingProgress.TotalCounter * (questions.Count() * answers.Count() - loadingProgress.TotalCounter));
                    
                    progressAction?.Invoke(loadingProgress);
                }
            }
        }

        private string ConcatQuestionAnswer(string question, string answer)
            => $"Frage:\n\"{question}\"\nAntwort:\n\"{answer}\"";
    }   
}
