using de.devcodemonkey.AIChecker.CoreBusiness.Models;

namespace de.devcodemonkey.AIChecker.UseCases
{
    public interface ISendQuestionsToLmsUseCase
    {
        Task ExecuteAsync(SendToLmsParams sendToLmsParams);
    }
}