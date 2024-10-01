namespace de.devcodemonkey.AIChecker.UseCases.Interfaces
{
    public interface IStartStopDatabaseUseCase
    {
        Task<bool> ExecuteAsync(bool start);
    }
}