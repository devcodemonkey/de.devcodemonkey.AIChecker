namespace de.devcodemonkey.AIChecker.UseCases.Interfaces
{
    public interface ICreateMoreQuestionsUseCase
    {        
        Task ExecuteAsync(string resultSet, string systemPromt, int maxTokens = -1, double temperture = 0.7);
    }
}