namespace de.devcodemonkey.AIChecker.UseCases.Interfaces
{
    public interface ISendAPIRequestToLmStudioAndSaveToDbUseCase
    {
        Task ExecuteAsync(List<string> UserMessages, string systemPromt, string resultSet, double temperture = 0.7);
    }
}