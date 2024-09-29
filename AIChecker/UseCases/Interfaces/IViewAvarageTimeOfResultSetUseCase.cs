namespace de.devcodemonkey.AIChecker.UseCases.Interfaces
{
    public interface IViewAverageTimeOfResultSetUseCase
    {
        Task<TimeSpan> ExecuteAsync(string resultSet);
    }
}