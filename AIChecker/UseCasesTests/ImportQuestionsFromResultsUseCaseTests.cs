using Microsoft.VisualStudio.TestTools.UnitTesting;
using de.devcodemonkey.AIChecker.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using de.devcodemonkey.AIChecker.DataStore.PostgreSqlEF;
using de.devcodemonkey.AIChecker.CoreBusiness.DbModels;
using de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;

namespace de.devcodemonkey.AIChecker.UseCases.Tests
{
    [TestClass()]
    public class ImportQuestionsFromResultsUseCaseTests
    {
        private readonly DbContextOptions<DbContext> _options = new DbContextOptionsBuilder<DbContext>()
                .UseInMemoryDatabase(databaseName: "AicheckerTestDatabase")
                .Options;

        [TestMethod()]
        public async Task ExecuteAsync_ValidJson_ShouldAddQuestionsToDatabase()
        {
            // Arrange
            string resultSet = "testResultSet";
            string category = "TestCategory";
            Guid answerId = Guid.NewGuid();

            ImportQuestionsFromResultsUseCase importQuestionsFromResultsUseCase =
                new ImportQuestionsFromResultsUseCase(new DefaultMethodesRepository(new AicheckerContext(_options)));

            var defaultMethodesRepository = new DefaultMethodesRepository(new AicheckerContext(_options));

            var resultSetEntity = await defaultMethodesRepository.AddAsync(
                new ResultSet
                {
                    ResultSetId = Guid.NewGuid(),
                    Value = resultSet
                });

            await defaultMethodesRepository.AddAsync(
                new Result
                {
                    ResultSetId = resultSetEntity.ResultSetId,
                    Message = "{\"questions\":[{\"Question\":\"Sample question?\"}]}",
                    AnswerId = answerId,
                    SystemPrompt = new SystemPrompt
                    {
                        SystemPromptId = Guid.NewGuid(),
                        Value = "SystemPrompt"
                    },
                    Model = new Model
                    {
                        ModelId = Guid.NewGuid(),
                        Value = "Model"
                    }
                });

            // Act
            await importQuestionsFromResultsUseCase.ExecuteAsync(resultSet, category);

            // Assert
            using (var context = new AicheckerContext(_options))
            {
                // Check if the QuestionCategory was added
                Assert.AreEqual(1, context.QuestionCategories.Count());
                Assert.AreEqual(category, context.QuestionCategories.First().Value);

                // Check if the Question was added
                Assert.AreEqual(1, context.Questions.Count());
                Assert.AreEqual("Sample question?", context.Questions.First().Value);
                Assert.AreEqual(answerId, context.Questions.First().AnswerId);
            }
        }
    }
}