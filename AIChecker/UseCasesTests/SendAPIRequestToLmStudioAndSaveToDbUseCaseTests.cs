using Microsoft.VisualStudio.TestTools.UnitTesting;
using de.devcodemonkey.AIChecker.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using de.devcodemonkey.AIChecker.DataSource.APIRequester;
using de.devcodemonkey.AIChecker.DataStore.SQLServerEF;

namespace de.devcodemonkey.AIChecker.UseCases.Tests
{
    [TestClass()]
    public class SendAPIRequestToLmStudioAndSaveToDbUseCaseTests
    {
        [TestMethod()]
        public async Task ExecuteAsyncTest()
        {
            SendAPIRequestToLmStudioAndSaveToDbUseCase sendAPIRequestToLmStudioAndSaveToDbUseCase =
                new SendAPIRequestToLmStudioAndSaveToDbUseCase(
                    new APIRequester(),
                    new DefaultMethodesRepository());

            await sendAPIRequestToLmStudioAndSaveToDbUseCase.ExecuteAsync(
                new List<string> { "Hello", "How are you?" },
                "You are a chatbot",
                "nothing set",
                0.7);
        }
    }
}