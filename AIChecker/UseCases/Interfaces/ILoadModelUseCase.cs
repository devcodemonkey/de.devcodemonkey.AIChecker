namespace de.devcodemonkey.AIChecker.UseCases.Interfaces
{
    public interface ILoadModelUseCase
    {
        Task<bool> ExecuteAsync(string modelName);
    }
}