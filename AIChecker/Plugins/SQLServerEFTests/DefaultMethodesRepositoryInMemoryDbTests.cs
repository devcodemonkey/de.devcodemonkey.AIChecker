using Microsoft.VisualStudio.TestTools.UnitTesting;
using de.devcodemonkey.AIChecker.DataStore.SQLServerEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using de.devcodemonkey.AIChecker.CoreBusiness.DbModels;
using de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;
using Microsoft.EntityFrameworkCore;

namespace de.devcodemonkey.AIChecker.DataStore.SQLServerEF.Tests
{
    [TestClass()]
    public class DefaultMethodesRepositoryInMemoryDbTests
    {
        private readonly DbContextOptions<DbContext> _options = new DbContextOptionsBuilder<DbContext>()
                .UseInMemoryDatabase(databaseName: "AicheckerTestDatabase")
                .Options;        

        [TestMethod()]
        public async Task AddAsyncTest()
        {
            IDefaultMethodesRepository defaultMethodesRepository = 
                new DefaultMethodesRepository(new AicheckerContext(_options));

            Question question = new Question
            {
                QuestionId = Guid.NewGuid(),
                Value = "Test",
                Answer = new Answer
                {
                    AnswerId = Guid.NewGuid(),
                    Value = "Test"
                }
            };

            await defaultMethodesRepository.AddAsync(question);
        }

        [TestMethod]
        public async Task ViewAvarageTimeOfResultSet_ShouldReturnCorrectAverageTime()
        {
            // Arrange
            var resultSetId = Guid.NewGuid();

            // Seed the in-memory database
            using (var context = new AicheckerContext(_options))
            {
                context!.Results.AddRange(new List<Result>
                {
                    new Result
                    {
                        ResultId = Guid.NewGuid(),
                        ResultSetId = resultSetId,
                        RequestStart = new DateTime(2024, 9, 12, 10, 0, 0),
                        RequestEnd = new DateTime(2024, 9, 12, 10, 30, 0)
                    }, // 30 minutes
                    new Result
                    {
                        ResultId = Guid.NewGuid(),
                        ResultSetId = resultSetId,
                        RequestStart = new DateTime(2024, 9, 12, 11, 0, 0),
                        RequestEnd = new DateTime(2024, 9, 12, 11, 15, 0)
                    }, // 15 minutes
                    new Result
                    {
                        ResultId = Guid.NewGuid(),
                        ResultSetId = resultSetId,
                        RequestStart = new DateTime(2024, 9, 12, 12, 0, 0),
                        RequestEnd = new DateTime(2024, 9, 12, 12, 45, 0)
                    }  // 45 minutes
                });

                await context.SaveChangesAsync();
            }

            // Act 
            var repository = new DefaultMethodesRepository(new AicheckerContext(_options));
            var result = await repository.ViewAvarageTimeOfResultSet(resultSetId);

            // Calculate expected average TimeSpan manually:
            var expectedTicks = (new TimeSpan(0, 30, 0).Ticks + new TimeSpan(0, 15, 0).Ticks + new TimeSpan(0, 45, 0).Ticks) / 3;
            var expectedAverageTimeSpan = TimeSpan.FromTicks(expectedTicks);

            // Assert
            Assert.AreEqual(expectedAverageTimeSpan, result);

        }
    }
}