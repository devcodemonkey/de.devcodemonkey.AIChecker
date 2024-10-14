using de.devcodemonkey.AIChecker.DataStore.SQLServerEF;
using de.devcodemonkey.AIChecker.CoreBusiness.DbModels;
using de.devcodemonkey.AIChecker.DataSource.APIRequester;
using LmsWrapper;
using de.devcodemonkey.AIChecker.CoreBusiness.Models;

namespace de.devcodemonkey.AIChecker.UseCases.Tests
{
    [TestClass()]
    public class CreatePromptRatingUseCaseTests
    {
        [TestMethod()]
        public async Task ExecuteAsyncTest()
        {
            // Arrange
            CreatePromptRatingUseCase createPromptRatingUseCase = new CreatePromptRatingUseCase(new DefaultMethodesRepository(new AicheckerContext()), new LoadUnloadLms(), new APIRequester());

            var defaultMethodesRepository = new DefaultMethodesRepository(new AicheckerContext());
            await defaultMethodesRepository.RecreateDatabaseAsync();
            var model1 = await defaultMethodesRepository.AddAsync(new Model { ModelId = Guid.NewGuid(), Value = "lmstudio-community/Phi-3.5-mini-instruct-GGUF" });
            var model2 = await defaultMethodesRepository.AddAsync(new Model { ModelId = Guid.NewGuid(), Value = "TheBloke/SauerkrautLM-7B-HerO-GGUF" });

            // Act
            await createPromptRatingUseCase.ExecuteAsync(new string[] { model1.Value, model2.Value }, 300, "resultSet", () => "systemPrompt", () => "message", () => 1, () => true, (ResponseData responseData) => { });

        }
    }
}