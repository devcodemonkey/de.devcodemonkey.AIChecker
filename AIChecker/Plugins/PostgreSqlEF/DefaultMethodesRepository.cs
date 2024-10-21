using de.devcodemonkey.AIChecker.CoreBusiness.DbModelInterfaces;
using de.devcodemonkey.AIChecker.CoreBusiness.DbModels;
using de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace de.devcodemonkey.AIChecker.DataStore.SQLServerEF
{
    public class DefaultMethodesRepository : IDefaultMethodesRepository
    {
        private readonly AicheckerContext _ctx;
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public DefaultMethodesRepository(AicheckerContext context)
        {
            _ctx = context;
        }

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

        public async Task<IEnumerable<T>> GetAllEntitiesAsync<T>() where T : class
        {
            await _semaphore.WaitAsync();
            try
            {
                return await _ctx.Set<T>().ToListAsync();
            }
            finally
            {
                _semaphore.Release();
            }
        }

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

            // No need to keep other non-DbContext related operations here
            return entity;
        }

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

        public async Task<Model> ViewModelOverValueAysnc(string value)
        {
            await _semaphore.WaitAsync();
            try
            {
                return await _ctx.Models.FirstOrDefaultAsync(m => m.Value == value);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<TTable> ViewOverValue<TTable>(string value) where TTable : class, IValue
        {
            await _semaphore.WaitAsync();
            try
            {
                return await _ctx.Set<TTable>().FirstOrDefaultAsync(t => t.Value == value);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<IEnumerable<Result>> ViewResultsOfResultSetAsync(Guid resultSetId)
        {
            await _semaphore.WaitAsync();
            try
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
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<Guid> GetResultSetIdByValueAsync(string resultSetValue)
        {
            await _semaphore.WaitAsync();
            try
            {
                var resultSet = await _ctx.ResultSets.FirstOrDefaultAsync(rs => rs.Value == resultSetValue);
                return resultSet!.ResultSetId;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<TimeSpan> ViewAverageTimeOfResultSet(Guid resultSetId)
        {
            await _semaphore.WaitAsync();
            try
            {
                List<long> timeDifferencesInTicks = await (from result in _ctx.Results
                                                           where result.ResultSetId == resultSetId
                                                           select (result.RequestEnd - result.RequestStart).Ticks)
                                                          .ToListAsync();

                if (timeDifferencesInTicks.Count == 0)
                    return TimeSpan.Zero;

                var averageTicks = timeDifferencesInTicks.Average();
                var averageTimeSpan = TimeSpan.FromTicks(Convert.ToInt64(averageTicks));

                return averageTimeSpan;
            }
            finally
            {
                _semaphore.Release();
            }
        }

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
}
