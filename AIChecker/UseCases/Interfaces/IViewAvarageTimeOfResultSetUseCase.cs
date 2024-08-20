namespace de.devcodemonkey.AIChecker.UseCases.Interfaces
{
    public interface IViewAvarageTimeOfResultSetUseCase
    {
        Task<TimeSpan> ExecuteAsync(string resultSet);
    }
}