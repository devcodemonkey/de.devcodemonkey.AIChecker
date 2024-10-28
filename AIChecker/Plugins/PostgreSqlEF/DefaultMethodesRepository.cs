using de.devcodemonkey.AIChecker.CoreBusiness.DbModelInterfaces;
using de.devcodemonkey.AIChecker.CoreBusiness.DbModels;
using de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;
using Microsoft.EntityFrameworkCore;


namespace de.devcodemonkey.AIChecker.DataStore.PostgreSqlEF;

    public class DefaultMethodesRepository : IDefaultMethodesRepository
{
    private readonly AicheckerContext _ctx;
    private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

    public DefaultMethodesRepository(AicheckerContext context)
    {
        _ctx = context;
    }

    // Write Operation with SemaphoreSlim
    public async Task RecreateDatabaseAsync()
    {
        await _semaphore.WaitAsync();
        try
        {
            await _ctx.Database.EnsureDeletedAsync();
            await _ctx.Database.MigrateAsync();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    // Read-Only Operation (SemaphoreSlim not necessary)
    public async Task<IEnumerable<T>> GetAllEntitiesAsync<T>() where T : class
    {
        return await _ctx.Set<T>().ToListAsync();
    }

    // Write Operation with SemaphoreSlim
    public async Task<T> AddAsync<T>(T entity) where T : class
    {
        await _semaphore.WaitAsync();
        try
        {
            _ctx.Set<T>().Add(entity);
            await _ctx.SaveChangesAsync();
        }
        finally
        {
            _semaphore.Release();
        }

        return entity;
    }

    // Write Operation with SemaphoreSlim
    public async Task<T> UpdateAsync<T>(T entity) where T : class
    {
        await _semaphore.WaitAsync();
        try
        {
            _ctx.Set<T>().Update(entity);
            await _ctx.SaveChangesAsync();
        }
        finally
        {
            _semaphore.Release();
        }

        return entity;
    }

    // Write Operation with SemaphoreSlim
    public async Task<IEnumerable<T>> AddAsync<T>(IEnumerable<T> entities) where T : class
    {
        await _semaphore.WaitAsync();
        try
        {
            _ctx.Set<T>().AddRange(entities);
            await _ctx.SaveChangesAsync();
        }
        finally
        {
            _semaphore.Release();
        }

        return entities;
    }

    // Write Operation with SemaphoreSlim
    public async Task<IEnumerable<T>> AddRangeAsync<T>(IEnumerable<T> entities) where T : class
    {
        await _semaphore.WaitAsync();
        try
        {
            _ctx.Set<T>().AddRange(entities);
            await _ctx.SaveChangesAsync();
        }
        finally
        {
            _semaphore.Release();
        }

        return entities;
    }

    // Write Operation with SemaphoreSlim
    public async Task<T> RemoveAsync<T>(T entity) where T : class
    {
        await _semaphore.WaitAsync();
        try
        {
            _ctx.Set<T>().Remove(entity);
            await _ctx.SaveChangesAsync();
        }
        finally
        {
            _semaphore.Release();
        }

        return entity;
    }

    // Write Operation with SemaphoreSlim
    public async Task RemoveAllEntitiesAsync<T>() where T : class
    {
        await _semaphore.WaitAsync();
        try
        {
            var entities = await _ctx.Set<T>().ToListAsync();
            _ctx.Set<T>().RemoveRange(entities);
            await _ctx.SaveChangesAsync();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    // Read-Only Operation (SemaphoreSlim not necessary)
    public async Task<Model> ViewModelOverValueAysnc(string value)
    {
        return await _ctx.Models.FirstOrDefaultAsync(m => m.Value == value);
    }

    // Read-Only Operation (SemaphoreSlim not necessary)
    public async Task<TTable> ViewOverValue<TTable>(string value) where TTable : class, IValue
    {
        return await _ctx.Set<TTable>().FirstOrDefaultAsync(t => t.Value == value);
    }

    // Read-Only Operation (SemaphoreSlim not necessary)
    public async Task<IEnumerable<Result>> ViewResultsOfResultSetAsync(Guid resultSetId)
    {
        return await _ctx.Results
            .Include(r => r.ResultSet)
            .Include(s => s.SystemPrompt)
            .Include(m => m.Model)
            .Include(r => r.RequestObject)
            .Include(r => r.RequestReason)
            .Include(r => r.PromptRatingRound)
            .Where(r => r.ResultSetId == resultSetId)
            .ToListAsync();
    }

    // Read-Only Operation (SemaphoreSlim not necessary)
    public async Task<Guid> GetResultSetIdByValueAsync(string resultSetValue)
    {
        var resultSet = await _ctx.ResultSets.FirstOrDefaultAsync(rs => rs.Value == resultSetValue);
        return resultSet!.ResultSetId;
    }

    // Read-Only Operation (SemaphoreSlim not necessary)
    public async Task<TimeSpan> ViewAverageTimeOfResultSet(Guid resultSetId)
    {
        List<long> timeDifferencesInTicks = await (from result in _ctx.Results
                                                   where result.ResultSetId == resultSetId
                                                   select (result.RequestEnd.HasValue && result.RequestStart.HasValue ? (result.RequestEnd.Value - result.RequestStart.Value).Ticks : 0))
                                                  .ToListAsync();

        if (timeDifferencesInTicks.Count == 0)
            return TimeSpan.Zero;

        var averageTicks = timeDifferencesInTicks.Average();
        var averageTimeSpan = TimeSpan.FromTicks(Convert.ToInt64(averageTicks));

        return averageTimeSpan;
    }

    // Write Operation with SemaphoreSlim
    public async Task RemoveResultSetAsync(Guid resultSetId)
    {
        await _semaphore.WaitAsync();
        try
        {
            var results = await _ctx.Results.Where(r => r.ResultSetId == resultSetId).ToListAsync();

            if (!results.Any())
            {
                Console.WriteLine("No results found for the specified ResultSet.");
                return;
            }

            var resultSet = await _ctx.ResultSets.FirstOrDefaultAsync(rs => rs.ResultSetId == resultSetId);

            _ctx.Results.RemoveRange(results);

            var model = await _ctx.Models.FirstOrDefaultAsync(m => m.ModelId == results.First().ModelId);
            var systemPrompt = await _ctx.SystemPrompts.FirstOrDefaultAsync(sp => sp.SystemPromptId == results.First().SystemPromptId);

            if (model != null && !await _ctx.Results.AnyAsync(r => r.ModelId == model.ModelId))
                _ctx.Models.Remove(model);
            if (systemPrompt != null && !await _ctx.Results.AnyAsync(r => r.SystemPromptId == systemPrompt.SystemPromptId))
                _ctx.SystemPrompts.Remove(systemPrompt);
            if (resultSet != null)
                _ctx.ResultSets.Remove(resultSet);

            await _ctx.SaveChangesAsync();
        }
        finally
        {
            _semaphore.Release();
        }
    }
}

