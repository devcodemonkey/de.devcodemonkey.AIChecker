using System.Linq.Expressions;

namespace de.devcodemonkey.AIChecker.DataStore.PostgreSqlEF.Contracts;

public interface IRespositoryBase<T>
{    
    void Create(T entity);
    void Update(T entity);
    void Delete(T entity);

    IQueryable<T> GetAll();
    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);

    Task CreateAsync(T entity);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> FindByConditionAsync(Expression<Func<T, bool>> expression);
}
