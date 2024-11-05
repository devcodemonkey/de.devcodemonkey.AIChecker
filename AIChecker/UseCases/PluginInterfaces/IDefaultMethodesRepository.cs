

using de.devcodemonkey.AIChecker.CoreBusiness.DbModelInterfaces;
using de.devcodemonkey.AIChecker.CoreBusiness.DbModels;

namespace de.devcodemonkey.AIChecker.UseCases.PluginInterfaces
{
    public interface IDefaultMethodesRepository
    {
        Task<IEnumerable<T>> AddAsync<T>(IEnumerable<T> entities) where T : class;
        Task<T> AddAsync<T>(T entity) where T : class;
        Task<IEnumerable<T>> GetAllEntitiesAsync<T>() where T : class;
        Task<T> RemoveAsync<T>(T entity) where T : class;
        Task<T> UpdateAsync<T>(T entity) where T : class;
        Task RemoveAllEntitiesAsync<T>() where T : class;
        Task<Model> ViewModelOverValueAysnc(string value);
        Task<TTable> ViewOverValue<TTable>(string value) where TTable : class, IValue;
        Task<TimeSpan> ViewAverageTimeOfResultSet(Guid resultSetId);
        Task RemoveResultSetAsync(Guid resultSetId);
        Task<Guid> GetResultSetIdByValueAsync(string resultSetValue);
        Task<IEnumerable<Result>> ViewResultsOfResultSetAsync(Guid resultSetId);
        Task RecreateDatabaseAsync();
        Task<IEnumerable<T>> AddRangeAsync<T>(IEnumerable<T> entities) where T : class;
        Task<IEnumerable<Question>> ViewQuestionAnswerByCategory(string category);
    }
}