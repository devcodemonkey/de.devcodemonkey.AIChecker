using de.devcodemonkey.AIChecker.CoreBusiness.Interfaces;
using de.devcodemonkey.AIChecker.CoreBusiness.Models;
using System.Net;

namespace de.devcodemonkey.AIChecker.DataSource.APIRequester.Tests
{
    [TestClass()]
    public class APIRequesterLiveTests
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