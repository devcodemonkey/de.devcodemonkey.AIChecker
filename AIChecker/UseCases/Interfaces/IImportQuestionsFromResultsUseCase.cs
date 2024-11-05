namespace de.devcodemonkey.AIChecker.UseCases.Interfaces
{
    public interface IImportQuestionsFromResultsUseCase
    {
        Task ExecuteAsync(string resultSet, string category);
        bool TryParseJson<T>(string json, out T? result);
    }
}