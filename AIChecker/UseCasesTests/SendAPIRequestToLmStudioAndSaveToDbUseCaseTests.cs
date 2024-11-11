using de.devcodemonkey.AIChecker.DataStore.PostgreSqlEF;
using de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;
using Microsoft.EntityFrameworkCore;
using de.devcodemonkey.AIChecker.CoreBusiness.Models;
using de.devcodemonkey.AIChecker.DataSource.APIRequester.Models;
using System.Net;
using Moq;
using de.devcodemonkey.AIChecker.CoreBusiness.DbModels;

namespace de.devcodemonkey.AIChecker.UseCases.Tests
{
    [TestClass()]
    public class SendAPIRequestToLmStudioAndSaveToDbUseCaseTests
    {
        private Mock<IAPIRequester> _mockApiRequester;
        private Mock<ISystemMonitor> _mockSystemMonitor;

        private SendAPIRequestToLmStudioAndSaveToDbUseCase _useCase;

        private DbContextOptions<DbContext> _dbContextOptions;

        [TestInitialize]
        public void SetUp()
        {
            _mockApiRequester = new Mock<IAPIRequester>();
            _mockSystemMonitor = new Mock<ISystemMonitor>();

            _dbContextOptions = new DbContextOptionsBuilder<DbContext>()
                .UseInMemoryDatabase(databaseName: $"AicheckerTestDb_{Guid.NewGuid()}")
                .Options;

            var context = new AicheckerContext(_dbContextOptions);
            context.Database.EnsureCreated();

            var defaultMethodesRepository = new DefaultMethodesRepository(context);

            _useCase = new SendAPIRequestToLmStudioAndSaveToDbUseCase(
                _mockApiRequester.Object,
                defaultMethodesRepository,
                _mockSystemMonitor.Object
            );
        }

        [TestMethod]
        public async Task ExecuteAsync_Should_SaveResultSetAndSendRequests()
        {
            // Arrange
            var sendToLmsParams = new SendToLmsParams
            {
                ResultSet = "Test ResultSet",
                SaveProcessUsage = false,
                RequestCount = 1,
                SystemPrompt = "Test System Prompt",
                UserMessage = "Test User Message",
                ResponseFormat = @"
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
                                  ""type"": ""string""
                                }
                              },
                              ""required"": [""Question""]
                            }
                          }
                        },
                        ""required"": [""questions""]
                      }
                    }
                }",
                MaxTokens = 100,
                Temperature = 0.7,
                Model = "gpt-4",
                Source = "TestSource",
                EnvironmentTokenName = "EnvToken"
            };

            var apiResult = new ApiResult<ResponseData>
            {
                StatusCode = HttpStatusCode.OK,
                Data = new ResponseData
                {
                    Object = "test-object",
                    Choices = new List<Choice> { new Choice { FinishReason = "stop" } }
                }
            };

            _mockApiRequester
                .Setup(api => api.SendChatRequestAsync(It.IsAny<RequestData>()))
                .ReturnsAsync(apiResult);

            using var context = new AicheckerContext(_dbContextOptions);

            // Act
            await _useCase.ExecuteAsync(sendToLmsParams);

            // Assert
            var resultSet = await context.ResultSets.FirstOrDefaultAsync();
            Assert.IsNotNull(resultSet);
            Assert.AreEqual("Test ResultSet", resultSet.Value);

            var result = await context.Results.FirstOrDefaultAsync();
            Assert.IsNotNull(result);
            Assert.AreEqual(sendToLmsParams.UserMessage, result.Asked);
            Assert.AreEqual(sendToLmsParams.ResponseFormat, result.ResponseFormat);

            _mockApiRequester.Verify(api => api.SendChatRequestAsync(It.IsAny<RequestData>()), Times.Once);
        }

        [TestMethod]
        public async Task ExecuteAsync_Should_SaveProcessUsage_When_Enabled()
        {
            // Arrange
            var sendToLmsParams = new SendToLmsParams
            {
                ResultSet = "Test ResultSet",
                SaveProcessUsage = true,
                SaveInterval = 2,
                WriteOutput = true,
                RequestCount = 1,
                SystemPrompt = "Test System Prompt",
                UserMessage = "Test User Message",
                ResponseFormat = null,
                Model = "gpt-4",
                MaxTokens = 100,
                Temperature = 0.5,
                Source = "TestSource",
                EnvironmentTokenName = "EnvToken"
            };

            var monitoringTaskCompletionSource = new TaskCompletionSource();
            _mockApiRequester
                .Setup(api => api.SendChatRequestAsync(It.IsAny<RequestData>()))
                .ReturnsAsync(new ApiResult<ResponseData>
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = new ResponseData
                    {
                        Object = "test-object",
                        Choices = new List<Choice> { new Choice { FinishReason = "stop" } }
                    }
                });


            using var context = new AicheckerContext(_dbContextOptions);

            // Act
            var task = _useCase.ExecuteAsync(sendToLmsParams);

            // Assert
            _mockSystemMonitor.Verify(monitor => monitor.MonitorPerformanceEveryXSecondsAsync(
                It.IsAny<Action<IEnumerable<SystemResourceUsage>>>(),
                sendToLmsParams.SaveInterval,
                It.IsAny<CancellationToken>(),
                sendToLmsParams.WriteOutput
            ), Times.Once);

            monitoringTaskCompletionSource.SetResult();
            await task;

            var resultSet = await context.ResultSets.FirstOrDefaultAsync();
            Assert.IsNotNull(resultSet);

            var result = await context.Results.FirstOrDefaultAsync();
            Assert.IsNotNull(result);

            _mockApiRequester.Verify(api => api.SendChatRequestAsync(It.IsAny<RequestData>()), Times.Once);
        }

    }
}