using de.devcodemonkey.AIChecker.DataStore.PostgreSqlEF;
using de.devcodemonkey.AIChecker.CoreBusiness.DbModels;
using de.devcodemonkey.AIChecker.DataSource.APIRequester;
using LmsWrapper;
using de.devcodemonkey.AIChecker.CoreBusiness.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

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
            await createPromptRatingUseCase.ExecuteAsync(
                new PromptRatingUseCaseParams()
                {
                    ModelNames = [model1.Value, model2.Value],
                    MaxTokens = 300,
                    ResultSet = "resultSet",
                    Description = "description text",
                    PromptRequirements = "prompt requierements description",
                    SystemPrompt = () => "You are helpful assistent",
                    Message = () => "Create me a poem",
                    RatingReason = () => ""
                },
                (Result result) => { }
            );
        }

        [TestMethod()]
        public async Task ExecuteAsync_With_ResponseFormat_and_ChatGPT()
        {
            // Arrange
            CreatePromptRatingUseCase createPromptRatingUseCase = new CreatePromptRatingUseCase(new DefaultMethodesRepository(new AicheckerContext(_options)), new LoadUnloadLms(), new APIRequester());

            var defaultMethodesRepository = new DefaultMethodesRepository(new AicheckerContext(_options));

            var model = await defaultMethodesRepository.AddAsync(new Model { ModelId = Guid.NewGuid(), Value = "gpt-4o-mini-2024-07-18" });

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
            try
            {
                JsonElement responseFormatElement = JsonSerializer.Deserialize<JsonElement>(responseFormat.Trim());
                Console.WriteLine("Parsing successful.");
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON Parsing error: {ex.Message}");
            }


            await createPromptRatingUseCase.ExecuteAsync(
                new PromptRatingUseCaseParams()
                {
                    ModelNames = [model.Value],
                    MaxTokens = 300,
                    ResultSet = "resultSet",                    
                    Description = "description text",
                    PromptRequirements = "prompt requierements description",
                    SystemPrompt = () => "You are helpful assistent",
                    Message = () => "Create me a poem",
                    Rating = () => 2,
                    RatingReason = () => "",
                    ResponseFormat = responseFormat,
                    NewImprovement = () => false
                },
                (Result result) => { }
            );
        }
    }
}