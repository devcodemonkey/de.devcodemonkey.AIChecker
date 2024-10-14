namespace de.devcodemonkey.AIChecker.UseCases.Interfaces
{
    public interface IUnloadModelUseCase
    {
        Task<bool> ExecuteAsync();
    }
}