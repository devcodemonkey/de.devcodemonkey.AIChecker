using de.devcodemonkey.AIChecker.DataStore.PostgreSqlEF;
using de.devcodemonkey.AIChecker.CoreBusiness.DbModels;
using de.devcodemonkey.AIChecker.DataSource.APIRequester;
using LmsWrapper;
using de.devcodemonkey.AIChecker.CoreBusiness.Models;
using Microsoft.EntityFrameworkCore;

namespace de.devcodemonkey.AIChecker.UseCases.Tests
{
    [TestClass()]
    public class CreatePromptRatingUseCaseTests
    {
        private readonly DbContextOptions<DbContext> _options = new DbContextOptionsBuilder<DbContext>()
                .UseInMemoryDatabase(databaseName: "AicheckerTestDatabase")
                .Options;

        [TestMethod()]
        public async Task ExecuteAsyncTest()
        {
            // Arrange
            CreatePromptRatingUseCase createPromptRatingUseCase = new CreatePromptRatingUseCase(new DefaultMethodesRepository(new AicheckerContext(_options)), new LoadUnloadLms(), new APIRequester());

            var defaultMethodesRepository = new DefaultMethodesRepository(new AicheckerContext(_options));

            var model1 = await defaultMethodesRepository.AddAsync(new Model { ModelId = Guid.NewGuid(), Value = "bartowski/Phi-3.5-mini-instruct_Uncensored-GGUF" });
            var model2 = await defaultMethodesRepository.AddAsync(new Model { ModelId = Guid.NewGuid(), Value = "TheBloke/em_german_mistral_v01-GGUF" });

            // Act
            await createPromptRatingUseCase.ExecuteAsync(new string[] { model1.Value, model2.Value }, 300, "resultSet", "prompt requierements description", () => "You are helpful assistent", () => "Create me a poem", () => "", () => 1, () => false, (Result result) => { });

        }
    }
}