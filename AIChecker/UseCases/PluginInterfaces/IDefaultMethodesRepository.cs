

using de.devcodemonkey.AIChecker.CoreBusiness.DbModelInterfaces;
using de.devcodemonkey.AIChecker.CoreBusiness.DbModels;

namespace de.devcodemonkey.AIChecker.UseCases.PluginInterfaces
{
    public interface IDefaultMethodesRepository
    {
        Task<List<T>> AddAsync<T>(List<T> entities) where T : class;
        Task<T> AddAsync<T>(T entity) where T : class;
        Task<List<T>> GetAllEntitiesAsync<T>() where T : class;
        Task<T> RemoveAsync<T>(T entity) where T : class;
        Task<T> UpdateAsync<T>(T entity) where T : class;
        Task RemoveAllEntitiesAsync<T>() where T : class;                                           
        Task<Model> ViewModelOverValue(string value);
        Task<TTable> ViewOverValue<TTable>(string value) where TTable : class, IValue;        
        Task<TimeSpan> ViewAvarageTimeOfResultSet(Guid resultSetId);
        Task RemoveResultSetAsync(Guid resultSetId);
        Task<Guid> GetResultSetIdByValueAsync(string resultSetValue);
        Task<IEnumerable<Result>> ViewResultsOfResultSetAsync(Guid resultSetId);
    }
}