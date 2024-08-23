namespace de.devcodemonkey.AIChecker.UseCases.Interfaces
{
    public interface ISendAPIRequestToLmStudioAndSaveToDbUseCase
    {
        Task ExecuteAsync(string userMessage, string systemPromt, string resultSet, int requestCount, int maxTokens, double temperture = 0.7);
    }
}