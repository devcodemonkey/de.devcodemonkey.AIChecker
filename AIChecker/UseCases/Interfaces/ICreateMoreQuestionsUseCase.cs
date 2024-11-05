using de.devcodemonkey.AIChecker.CoreBusiness.Models;

namespace de.devcodemonkey.AIChecker.UseCases.Interfaces
{
    public interface ICreateMoreQuestionsUseCase
    {        
        Task ExecuteAsync(MoreQuestionsUseCaseParams moreQuestionsUseCaseParams, CreateMoreQuestionsUseCase.StatusHandler? statusHandler = null);
    }
}