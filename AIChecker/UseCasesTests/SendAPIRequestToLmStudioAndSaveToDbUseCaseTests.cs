using Microsoft.VisualStudio.TestTools.UnitTesting;
using de.devcodemonkey.AIChecker.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using de.devcodemonkey.AIChecker.DataSource.APIRequester;
using de.devcodemonkey.AIChecker.DataStore.SQLServerEF;
using Microsoft.Extensions.Options;
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