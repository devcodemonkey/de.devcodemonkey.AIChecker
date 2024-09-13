using de.devcodemonkey.AIChecker.CoreBusiness.DbModels;
using de.devcodemonkey.AIChecker.DataSource.APIRequester;
using de.devcodemonkey.AIChecker.DataSource.SystemMonitor;
using de.devcodemonkey.AIChecker.DataStore.SQLServerEF;
using de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;
using Microsoft.EntityFrameworkCore;

namespace de.devcodemonkey.AIChecker.UseCases.Tests
{
    [TestClass()]
    public class SendAPIRequestToLmStudioAndSaveToDbUseCaseTests
    {
        private readonly DbContextOptions<DbContext> _options = new DbContextOptionsBuilder<DbContext>()
                .UseInMemoryDatabase(databaseName: "AicheckerTestDatabase")
                .Options;


        [TestMethod()]
        public async Task ExecuteAsyncTest()
        {
            // Arrange
            var defaultMethodesRepository = new DefaultMethodesRepository(new AicheckerContext(_options));

            SendAPIRequestToLmStudioAndSaveToDbUseCase sendAPIRequestToLmStudioAndSaveToDbUseCase =
                new SendAPIRequestToLmStudioAndSaveToDbUseCase(
                    new APIRequester(),
                    defaultMethodesRepository,
                    new SystemMonitor());

            // Act
            await sendAPIRequestToLmStudioAndSaveToDbUseCase.ExecuteAsync(
                "How are you",
                "You are a chatbot",
                $"Testcase {DateTime.Now}");

            // Assert

            var systemUsages = await defaultMethodesRepository.GetAllEntitiesAsync<SystemResourceUsage>();
            Assert.IsTrue(systemUsages.ToArray().Count() > 0);

        }
    }
}