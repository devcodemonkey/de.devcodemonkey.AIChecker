using de.devcodemonkey.AIChecker.DataStore.PostgreSqlEF.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace de.devcodemonkey.AIChecker.DataStore.PostgreSqlEF.Respository;

public abstract class RepositoryBase<T>(AicheckerContext ctx) : IRespositoryBase<T> where T : class
{
    public void Create(T entity) => ctx.Set<T>().Add(entity);
    public void Delete(T entity) => ctx.Set<T>().Remove(entity);
    public void Update(T entity) => ctx.Set<T>().Update(entity);

    public IQueryable<T> GetAll() => ctx.Set<T>();
    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression) => ctx.Set<T>().Where(expression);


    public async Task CreateAsync(T entity) => await ctx.Set<T>().AddAsync(entity);

    public async Task<IEnumerable<T>> GetAllAsync() => await ctx.Set<T>().ToListAsync();
    public async Task<IEnumerable<T>> FindByConditionAsync(Expression<Func<T, bool>> expression) 
        => await ctx.Set<T>().Where(expression).ToListAsync();
}