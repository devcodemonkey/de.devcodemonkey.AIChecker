using Microsoft.VisualStudio.TestTools.UnitTesting;
using de.devcodemonkey.AIChecker.DataSource.APIRequester;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using de.devcodemonkey.AIChecker.CoreBusiness.Interfaces;
using System.Net;
using de.devcodemonkey.AIChecker.CoreBusiness.Models;

namespace de.devcodemonkey.AIChecker.DataSource.APIRequester.Tests
{
    [TestClass()]
    public class APIRequesterTests
    {
        // test will only pass if the LM Studio API is up and running
        [TestMethod()]
        public async Task SendChatRequestAsyncTest_LmStudioAPI()
        {
            APIRequester apiRequester = new APIRequester();

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

            var result = await apiRequester.SendChatRequestAsync(messages, maxTokens: -1);

            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(result.Data);
        }

        // test will only pass if the system environment variable OPEN_AI_TOKEN is set
        [TestMethod()]
        public async Task SendChatRequestAsyncTest_OpenAiAPI()
        {
            APIRequester apiRequester = new APIRequester();

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

            var result = await apiRequester.SendChatRequestAsync(
                messages,
                source: "https://api.openai.com/v1/chat/completions",
                model: "gpt-4o-mini",
                environmentTokenName: "OPEN_AI_TOKEN");

            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(result.Data);
        }
    }
}