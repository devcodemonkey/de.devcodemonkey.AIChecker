
namespace de.devcodemonkey.AIChecker.UseCases.PluginInterfaces
{
    public interface IDefaultMethodesRepository
    {
        Task<List<T>> AddAsync<T>(List<T> entities) where T : class;
        Task<T> AddAsync<T>(T entity) where T : class;
        Task<List<T>> GetAllEntitiesAsync<T>() where T : class;
        Task<T> RemoveAsync<T>(T entity) where T : class;
        Task<T> UpdateAsync<T>(T entity) where T : class;
    }
}