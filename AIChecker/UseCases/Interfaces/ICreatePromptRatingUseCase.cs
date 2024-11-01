using de.devcodemonkey.AIChecker.CoreBusiness.DbModels;
using de.devcodemonkey.AIChecker.CoreBusiness.Models;

namespace de.devcodemonkey.AIChecker.UseCases.Interfaces
{
    public interface ICreatePromptRatingUseCase
    {   
        Task ExecuteAsync(PromptRatingUseCaseParams promptParams, Action<Result> displayResult, CreatePromptRatingUseCase.StatusHandler? statusHandler = null);
    }
}