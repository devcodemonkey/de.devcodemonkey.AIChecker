using de.devcodemonkey.AIChecker.CoreBusiness.DbModels;
using de.devcodemonkey.AIChecker.CoreBusiness.Models;

namespace de.devcodemonkey.AIChecker.UseCases.Interfaces
{
    public interface ICreatePromptRatingUseCase
    {        
        Task ExecuteAsync(string[] modelNames, int maxTokens, string resultSet, Func<string> systemPrompt, Func<string> message, Func<int> ranking, Func<bool> newImprovement, Action<Result> DisplayResult);
    }
}