namespace de.devcodemonkey.AIChecker.UseCases.Interfaces
{
    public interface IDeleteResultSetUseCase
    {
        Task ExecuteAsync(string resultSet);
    }
}