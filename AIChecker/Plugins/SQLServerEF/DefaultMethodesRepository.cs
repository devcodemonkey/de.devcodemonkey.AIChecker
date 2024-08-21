using de.devcodemonkey.AIChecker.CoreBusiness.DbModelInterfaces;
using de.devcodemonkey.AIChecker.CoreBusiness.DbModels;
using de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace de.devcodemonkey.AIChecker.DataStore.SQLServerEF
{
    public class DefaultMethodesRepository : IDefaultMethodesRepository
    {
        public async Task<List<T>> GetAllEntitiesAsync<T>() where T : class
        {
            using (var ctx = new AicheckerContext())
            {
                return await ctx.Set<T>().ToListAsync();
            }
        }

        public async Task<T> AddAsync<T>(T entity) where T : class
        {
            using (var ctx = new AicheckerContext())
            {
                ctx.Set<T>().Add(entity);
                await ctx.SaveChangesAsync();
                return entity;
            }
        }

        public async Task<T> UpdateAsync<T>(T entity) where T : class
        {
            using (var ctx = new AicheckerContext())
            {
                ctx.Set<T>().Update(entity);
                await ctx.SaveChangesAsync();
                return entity;
            }
        }

        public async Task<List<T>> AddAsync<T>(List<T> entities) where T : class
        {
            using (var ctx = new AicheckerContext())
            {
                ctx.Set<T>().AddRange(entities);
                await ctx.SaveChangesAsync();
                return entities;
            }
        }

        public async Task<T> RemoveAsync<T>(T entity) where T : class
        {
            using (var ctx = new AicheckerContext())
            {
                ctx.Set<T>().Remove(entity);
                await ctx.SaveChangesAsync();
                return entity;
            }
        }

        public async Task RemoveAllEntitiesAsync<T>() where T : class
        {
            using (var context = new AicheckerContext())
            {
                var entities = await context.Set<T>().ToListAsync();
                context.Set<T>().RemoveRange(entities);
                context.SaveChanges();
            }
        }

        public async Task<Model> ViewModelOverValue(string value)
        {
            using (var ctx = new AicheckerContext())
            {
                var model = await ctx.Models.FirstOrDefaultAsync(m => m.Value == value);
                return model;
            }
        }

        public async Task<TTable> ViewOverValue<TTable>(string value) where TTable : class, IValue
        {
            using (var ctx = new AicheckerContext())
            {
                var table = await ctx.Set<TTable>().FirstOrDefaultAsync(t => t.Value == value);
                return table;
            }
        }

        public async Task<TimeSpan> ViewAvarageTimeOfResultSet(string resultSetValue)
        {
            using (var ctx = new AicheckerContext())
            {
                var resultSet = await ctx.ResultSets.FirstOrDefaultAsync(rs => rs.Value == resultSetValue);
                return await ViewAvarageTimeOfResultSet(resultSet.ResultSetId);
            }
        }

        public async Task<TimeSpan> ViewAvarageTimeOfResultSet(Guid resultSetId)
        {
            using (var ctx = new AicheckerContext())
            {
                var timeDifferencesInTicks = await (from result in ctx.Results
                                                    where result.ResultSetId == resultSetId
                                                    select (result.RequestEnd - result.RequestStart).Ticks).ToListAsync();

                // Perform the average calculation on the client side
                var averageTicks = timeDifferencesInTicks.Average(); // Calculate average on the client side
                var averageTimeSpan = TimeSpan.FromTicks(Convert.ToInt64(averageTicks)); // Convert to TimeSpan

                return averageTimeSpan;
            }
        }

    }
}