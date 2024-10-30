using de.devcodemonkey.AIChecker.CoreBusiness.DbModels;
using de.devcodemonkey.AIChecker.CoreBusiness.Models;

namespace de.devcodemonkey.AIChecker.UseCases.Interfaces
{
    public interface ICreatePromptRatingUseCase
    {        
        Task ExecuteAsync(string[] modelNames, int maxTokens, string resultSet, string description, string promptRequierements, Func<string> systemPrompt, Func<string> message, Func<string> ratingReason, Func<int> rating, Func<bool> newImprovement, Action<Result> DisplayResult, CreatePromptRatingUseCase.StatusHandler? statusHandler = null);
    }
}