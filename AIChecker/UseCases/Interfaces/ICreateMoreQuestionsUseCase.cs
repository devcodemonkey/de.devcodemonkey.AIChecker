namespace de.devcodemonkey.AIChecker.UseCases.Interfaces
{
    public interface ICreateMoreQuestionsUseCase
    {
        Task ExecuteAsync(string systemPromt, string resultSet, double temperture = 0.7);
    }
}