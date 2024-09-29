using de.devcodemonkey.AIChecker.CoreBusiness.DbModelInterfaces;
using de.devcodemonkey.AIChecker.CoreBusiness.DbModels;
using de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;
using Microsoft.EntityFrameworkCore;

namespace de.devcodemonkey.AIChecker.DataStore.SQLServerEF
{
    public class DefaultMethodesRepository : IDefaultMethodesRepository
    {
        private readonly AicheckerContext _ctx;

        public DefaultMethodesRepository(AicheckerContext context)
        {
            _ctx = context;
        }

        public DefaultMethodesRepository()
        {
            _ctx = new AicheckerContext();
        }

        public async Task<IEnumerable<T>> GetAllEntitiesAsync<T>() where T : class
        {
            return await _ctx.Set<T>().ToListAsync();
        }

        public async Task<T> AddAsync<T>(T entity) where T : class
        {
            _ctx.Set<T>().Add(entity);
            await _ctx.SaveChangesAsync();
            return entity;
        }

        public async Task<T> UpdateAsync<T>(T entity) where T : class
        {
            _ctx.Set<T>().Update(entity);
            await _ctx.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<T>> AddAsync<T>(IEnumerable<T> entities) where T : class
        {
            _ctx.Set<T>().AddRange(entities);
            await _ctx.SaveChangesAsync();
            return entities;
        }

        public async Task<T> RemoveAsync<T>(T entity) where T : class
        {
            _ctx.Set<T>().Remove(entity);
            await _ctx.SaveChangesAsync();
            return entity;
        }

        public async Task RemoveAllEntitiesAsync<T>() where T : class
        {
            var entities = await _ctx.Set<T>().ToListAsync();
            _ctx.Set<T>().RemoveRange(entities);
            await _ctx.SaveChangesAsync();
        }

        public async Task<Model> ViewModelOverValue(string value)
        {
            return await _ctx.Models.FirstOrDefaultAsync(m => m.Value == value);
        }

        public async Task<TTable> ViewOverValue<TTable>(string value) where TTable : class, IValue
        {
            return await _ctx.Set<TTable>().FirstOrDefaultAsync(t => t.Value == value);
        }

        public async Task<IEnumerable<Result>> ViewResultsOfResultSetAsync(Guid resultSetId)
        {
            return await _ctx.Results
                .Include(r => r.ResultSet)
                .Include(s => s.SystemPrompt)
                .Include(m => m.Model)
                .Include(r => r.RequestObject)
                .Include(r => r.RequestReason)
                .Where(r => r.ResultSetId == resultSetId)
                .ToListAsync();
        }

        public async Task<Guid> GetResultSetIdByValueAsync(string resultSetValue)
        {
            var resultSet = await _ctx.ResultSets.FirstOrDefaultAsync(rs => rs.Value == resultSetValue);
            return resultSet!.ResultSetId;
        }

        public async Task<TimeSpan> ViewAverageTimeOfResultSet(Guid resultSetId)
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

        public async Task RemoveResultSetAsync(Guid resultSetId)
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
            var systemPrompt = await _ctx.SystemPromts.FirstOrDefaultAsync(sp => sp.SystemPromptId == results.First().SystemPromptId);

            if (model != null && !await _ctx.Results.AnyAsync(r => r.ModelId == model.ModelId))
                _ctx.Models.Remove(model);
            if (systemPrompt != null && !await _ctx.Results.AnyAsync(r => r.SystemPromptId == systemPrompt.SystemPromptId))
                _ctx.SystemPromts.Remove(systemPrompt);
            if (resultSet != null)
                _ctx.ResultSets.Remove(resultSet);

            await _ctx.SaveChangesAsync();
        }

        public Task RecreateDatabaseAsync()
        {
            throw new NotImplementedException();
        }
    }
}
