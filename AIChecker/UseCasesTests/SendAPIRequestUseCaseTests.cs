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
        public async Task ExecuteAsyncTest_LmStudioAPI()
        {
            SendAPIRequestUseCase sendAPIRequestUseCase = new SendAPIRequestUseCase(
                new DataSource.APIRequester.APIRequester());

            List<IMessage> messages = new List<IMessage>
            {
                new Message
                {
                    Role = "system",
                    Content = "You are a helpful assistent"
                },
                new Message
                {
                    Role = "user",
                    Content = "Gib mir 2 zufallsfragen aus dem IT-Support"
                }
            };

            var result = await sendAPIRequestUseCase.ExecuteAsync(messages, maxTokens: 20);

            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(result.Data);
        }

        // test will only pass if the system environment variable OPEN_AI_TOKEN is set
        [TestMethod()]
        public async Task ExecuteAsyncTest_OpenAiAPI()
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

            var result = await sendAPIRequestUseCase.ExecuteAsync(
                messages, 
                source: "https://api.openai.com/v1/chat/completions",
                model: "gpt-4o-mini",
                environmentTokenName: "OPEN_AI_TOKEN");

            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(result.Data);
        }
    }
}