using de.devcodemonkey.AIChecker.CoreBusiness.DbModels;
using de.devcodemonkey.AIChecker.DataStore.PostgreSqlEF;
using Microsoft.EntityFrameworkCore;

namespace de.devcodemonkey.AIChecker.UseCases.Tests
{
    [TestClass()]
    public class DeleteResultSetUseCaseTests
    {
        private readonly DbContextOptions<DbContext> _options = new DbContextOptionsBuilder<DbContext>()
                .UseInMemoryDatabase(databaseName: "AicheckerTestDatabase")
                .Options;

        [TestMethod()]
        public async Task ExecuteAsyncTest_DeleteOverId()
        {
            // Arrange
            DeleteResultSetUseCase deleteResultSetUseCase = new DeleteResultSetUseCase(new DefaultMethodesRepository(new AicheckerContext(_options)));
            var resultSetId = Guid.NewGuid();

            // Seed the in-memory database
            using (var context = new AicheckerContext(_options))
            {
                context!.Results.AddRange(new List<Result>
                {
                    new Result
                    {
                        ResultId = Guid.NewGuid(),
                        ResultSet = new ResultSet {
                            ResultSetId = resultSetId,
                            Value = "ResultsetValue"+ Guid.NewGuid().ToString(),
                            SystemResourceUsages = new List<SystemResourceUsage>
                            {
                                new SystemResourceUsage
                                {
                                    SystemResourceUsageId = Guid.NewGuid(),
                                    ProcessId = 1,
                                    ProcessName = "Test",
                                    CpuUsage = 5,
                                    MemoryUsage = 5
                                },
                                new SystemResourceUsage
                                {
                                    SystemResourceUsageId = Guid.NewGuid(),
                                    ProcessId = 2,
                                    ProcessName = "Test",
                                    CpuUsage = 5,
                                    MemoryUsage = 5
                                }
                            }
                        },
                        RequestStart = new DateTime(2024, 9, 12, 10, 0, 0),
                        RequestEnd = new DateTime(2024, 9, 12, 10, 30, 0)
                    }
                });

                context!.SaveChanges();
            }

            // Act, Assert
            var repository = new DefaultMethodesRepository(new AicheckerContext(_options));
            await deleteResultSetUseCase.ExecuteAsync(resultSetId.ToString());
        }

        [TestMethod()]
        public async Task ExecuteAsyncTest_DeleteOverValue()
        {
            // Arrange
            DeleteResultSetUseCase deleteResultSetUseCase = new DeleteResultSetUseCase(new DefaultMethodesRepository(new AicheckerContext(_options)));
            var resultSetValue = "ResultsetValue" + Guid.NewGuid().ToString();
            // Seed the in-memory database
            using (var context = new AicheckerContext(_options))
            {
                context!.Results.AddRange(new List<Result>
                {
                    new Result
                    {
                        ResultId = Guid.NewGuid(),
                        ResultSet = new ResultSet {
                            ResultSetId = Guid.NewGuid(),
                            Value = resultSetValue,
                            SystemResourceUsages = new List<SystemResourceUsage>
                            {
                                new SystemResourceUsage
                                {
                                    SystemResourceUsageId = Guid.NewGuid(),
                                    ProcessId = 1,
                                    ProcessName = "Test",
                                    CpuUsage = 5,
                                    MemoryUsage = 5
                                },
                                new SystemResourceUsage
                                {
                                    SystemResourceUsageId = Guid.NewGuid(),
                                    ProcessId = 2,
                                    ProcessName = "Test",
                                    CpuUsage = 5,
                                    MemoryUsage = 5
                                }
                            }
                        },
                        RequestStart = new DateTime(2024, 9, 12, 10, 0, 0),
                        RequestEnd = new DateTime(2024, 9, 12, 10, 30, 0)
                    }
                });
                context!.SaveChanges();
            }
            // Act, Assert
            var repository = new DefaultMethodesRepository(new AicheckerContext(_options));
            await deleteResultSetUseCase.ExecuteAsync(resultSetValue);            
        }
    }
}