using de.devcodemonkey.AIChecker.DataSource.APIRequester;
using de.devcodemonkey.AIChecker.DataStore.SQLServerEF;
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
            SendAPIRequestToLmStudioAndSaveToDbUseCase sendAPIRequestToLmStudioAndSaveToDbUseCase =
                new SendAPIRequestToLmStudioAndSaveToDbUseCase(
                    new APIRequester(),
                    new DefaultMethodesRepository(new AicheckerContext(_options)));

            await sendAPIRequestToLmStudioAndSaveToDbUseCase.ExecuteAsync(
                "How are you",
                "You are a chatbot",
                $"Testcase {DateTime.Now}");
        }
    }
}