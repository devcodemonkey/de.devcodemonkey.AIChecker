namespace de.devcodemonkey.AIChecker.UseCases.Interfaces
{
    public interface IViewResultSetsUseCase
    {
        Task<IEnumerable<string>> ExecuteAsync();
    }
}