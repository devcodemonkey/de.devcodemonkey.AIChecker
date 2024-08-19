using Microsoft.VisualStudio.TestTools.UnitTesting;
using de.devcodemonkey.AIChecker.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using de.devcodemonkey.AIChecker.CoreBusiness.Interfaces;
using de.devcodemonkey.AIChecker.CoreBusiness.Models;
using System.Net;

namespace de.devcodemonkey.AIChecker.UseCases.Tests
{
    [TestClass()]
    public class SendAPIRequestUseCaseTests
    {
        // test will only pass if the LM Studio API is up and running
        [TestMethod()]
        public async Task ExecuteAsyncTest()
        {
            SendAPIRequestUseCase sendAPIRequestUseCase = new SendAPIRequestUseCase(
                new DataSource.APIRequester.APIRequester());

            List<IMessage> messages = new List<IMessage>
            {
                new Message
                {
                    Role = "system",
                    Content = "You are a chatbot"
                },
                new Message
                {
                    Role = "user",
                    Content = "Hello"
                }
            };

            var result = await sendAPIRequestUseCase.ExecuteAsync(messages);

            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);
        }
    }
}