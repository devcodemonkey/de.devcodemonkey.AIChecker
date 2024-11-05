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
        private DbContextOptions<DbContext> _options;
        private DefaultMethodesRepository _defaultMethodesRepository;

        [TestInitialize]
        public async Task TestInitialize()
        {
            _options = new DbContextOptionsBuilder<DbContext>()
                .UseInMemoryDatabase(databaseName: "AicheckerTestDatabase")
                .Options;

            using (var context = new AicheckerContext(_options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

            _defaultMethodesRepository = new DefaultMethodesRepository(new AicheckerContext(_options));
        }

        [TestMethod()]
        public async Task ExecuteAsync_ValidJson_ShouldAddQuestionsToDatabase()
        {
            // Arrange
            string resultSet = "testResultSet";
            string category = "TestCategory";
            Guid answerId = Guid.NewGuid();

            ImportQuestionsFromResultsUseCase importQuestionsFromResultsUseCase =
                new ImportQuestionsFromResultsUseCase(new DefaultMethodesRepository(new AicheckerContext(_options)));



            var resultSetEntity = await _defaultMethodesRepository.AddAsync(
                new ResultSet
                {
                    ResultSetId = Guid.NewGuid(),
                    Value = resultSet
                });

            await _defaultMethodesRepository.AddAsync(
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

        [TestMethod]
        public async Task ExecuteAsync_InvalidJson_ShouldNotAddQuestionsToDatabase()
        {
            // Arrange
            string resultSet = "testResultSet";
            string category = "TestCategory";
            Guid answerId = Guid.NewGuid();

            // Seed data in the in-memory database
            var resultSetEntity = await _defaultMethodesRepository.AddAsync(new ResultSet { ResultSetId = Guid.NewGuid(), Value = resultSet });
            await _defaultMethodesRepository.AddAsync(
                new Result
                {
                    ResultSetId = resultSetEntity.ResultSetId,
                    Message = "Invalid JSON",
                    AnswerId = answerId,
                    Model = new Model
                    {
                        ModelId = Guid.NewGuid(),
                        Value = "Model"
                    },
                });

            // Create an instance of ImportQuestionsFromResultsUseCase
            var useCase = new ImportQuestionsFromResultsUseCase(_defaultMethodesRepository);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<Exception>(async () =>
                await useCase.ExecuteAsync(resultSet, category));

            // Verify that nothing was added to the Questions or QuestionCategories tables
            using (var context = new AicheckerContext(_options))
            {
                Assert.AreEqual(0, context.QuestionCategories.Count());
                Assert.AreEqual(0, context.Questions.Count());
            }
        }
    }
}