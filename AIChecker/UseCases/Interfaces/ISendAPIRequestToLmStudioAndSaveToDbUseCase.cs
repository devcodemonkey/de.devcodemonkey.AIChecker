namespace de.devcodemonkey.AIChecker.UseCases.Interfaces
{
    public interface ISendAPIRequestToLmStudioAndSaveToDbUseCase
    {        
        Task ExecuteAsync(string userMessage, string systemPromt, string resultSet, int requestCount = 1, int maxTokens = -1, double temperture = 0.7, bool saveProcessUsage = true, int saveInterval = 5);
    }
}