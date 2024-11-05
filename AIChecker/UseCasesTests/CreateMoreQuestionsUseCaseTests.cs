using Microsoft.VisualStudio.TestTools.UnitTesting;
using de.devcodemonkey.AIChecker.UseCases;
using de.devcodemonkey.AIChecker.DataSource.APIRequester;
using de.devcodemonkey.AIChecker.DataStore.PostgreSqlEF;
using Microsoft.EntityFrameworkCore;
using de.devcodemonkey.AIChecker.CoreBusiness.DbModels;
using de.devcodemonkey.AIChecker.CoreBusiness.Models;
using System.Diagnostics;

namespace de.devcodemonkey.AIChecker.UseCases.Tests
{
    [TestClass()]
    public class CreateMoreQuestionsUseCaseTests
    {
        private readonly DbContextOptions<DbContext> _options = new DbContextOptionsBuilder<DbContext>()
                .UseInMemoryDatabase(databaseName: "AicheckerTestDatabase")
                .Options;

        [TestMethod()]
        public async Task CreateMoreQuestionsUseCaseTest()
        {
            CreateMoreQuestionsUseCase createMoreQuestionsUseCase = new CreateMoreQuestionsUseCase(
                new APIRequester(), new DefaultMethodesRepository(new AicheckerContext()));


            // deactivating this test because it is a real request to the API
            // can be activated if needed


            //            await createMoreQuestionsUseCase.ExecuteAsync(
            //                "Test: More Questions",
            //                @"Du Antwortest nur im json-Format.  Das zu verwendende Format ist:
            //[
            //  {
            //    ""Question"": ""Beispielfrage 1"",    
            //  },
            //  {
            //    ""Question"" ""Beispielfrage 2"",    
            //  },
            //]
            //Du bist eine hilfesuchende Person, die Fragen  an einen IT-Support sendet.
            //Erstelle mir 10 Fragen auf Grundlage des folgenden Satzes:"
            //                );
        }

        [TestMethod()]
        public async Task ExecuteAsyncTest()
        {
            // Arrange
            CreateMoreQuestionsUseCase createMoreQuestionsUseCase = new CreateMoreQuestionsUseCase(
                new APIRequester(), new DefaultMethodesRepository(new AicheckerContext(_options)));

            var defaultMethodesRepository = new DefaultMethodesRepository(new AicheckerContext(_options));

            var model = await defaultMethodesRepository.AddAsync(new Model { ModelId = Guid.NewGuid(), Value = "gpt-4o-mini-2024-07-18" });

            var answer = new Answer { AnswerId = Guid.NewGuid(), Value = "Test Answer" };

            var question = new Question
            {
                QuestionId = Guid.NewGuid(),
                Value = "Test Question",
                Category = new QuestionCategory { QuestionCategoryId = Guid.NewGuid(), Value = "Test Category" },
                Answer = answer
            };

            await defaultMethodesRepository.AddAsync(question);

            // Act
            var responseFormat = @"
{
    ""type"": ""json_schema"",
    ""json_schema"": {
      ""name"": ""questions"",
      ""schema"": {
        ""type"": ""object"",
        ""properties"": {
          ""questions"": {
            ""type"": ""array"",
            ""items"": {
              ""type"": ""object"",
              ""properties"": {
                ""Question"": {
                  ""type"": ""string"",
                  ""description"": ""The text of the question.""
                }
              },
              ""required"": [""Question""],
              ""additionalProperties"": false
            }
          }
        },
        ""required"": [""questions""],
        ""additionalProperties"": false
      },
      ""strict"": true
    }
  }
";

            await createMoreQuestionsUseCase.ExecuteAsync(
                new MoreQuestionsUseCaseParams
                {
                    Category = "Test Category",
                    Model = "gpt-4o-mini-2024-07-18",
                    ResultSet = "Test ResultSet",
                    SystemPrompt = "Test System Prompt",
                    ResponseFormat = responseFormat
                }
            );

            // Assert
            var result = (await defaultMethodesRepository.GetAllEntitiesAsync<Result>()).FirstOrDefault();
            var systemPrompt = (await defaultMethodesRepository.GetAllEntitiesAsync<SystemPrompt>()).FirstOrDefault();

            Assert.AreEqual(answer.AnswerId, result!.AnswerId);

            Assert.IsNotNull(result!.ResultId);
            Assert.IsNotNull(result!.RequestId);
            Assert.IsNotNull(result!.RequestReasonId);
            Assert.IsNotNull(result!.RequestId);

            Assert.IsTrue(result!.Asked!.Contains(question.Value));
            Assert.IsTrue(result!.Asked!.Contains(answer.Value));
            Debug.WriteLine($"Asked: {result!.Asked}");

            Assert.IsNotNull(result!.Message);
            Debug.WriteLine($"Message: {result!.Message}");

            Assert.AreEqual(systemPrompt!.SystemPromptId, result!.SystemPromptId);

            Assert.AreEqual(0, result!.Temperature);

            Assert.AreEqual(null, result!.MaxTokens);

            Assert.IsNotNull(result!.PromptTokens);
            Assert.IsNotNull(result!.CompletionTokens);
            Assert.IsNotNull(result!.TotalTokens);

            Assert.IsNotNull(result!.RequestStart);
            Assert.IsNotNull(result!.RequestEnd);

            Assert.AreEqual(responseFormat, result!.ResponseFormat);
        }

    }
}
