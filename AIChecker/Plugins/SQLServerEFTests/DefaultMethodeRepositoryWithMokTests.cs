using de.devcodemonkey.AIChecker.CoreBusiness.DbModels;
using Moq;
using Moq.EntityFrameworkCore;

namespace SQLServerEFTests
{
    [TestClass]
    public class DefaultMethodeRepositoryWithMokTests
    {
        [TestMethod]
        public async Task ViewAvarageTimeOfResultSet_ShouldReturnCorrectAverageTime()
        {
            // Arrange
            var resultSetId = Guid.NewGuid();

            IList<Result> results = new List<Result>
            {
                new Result
                {
                    ResultId = Guid.NewGuid(),
                    ResultSetId = resultSetId,
                    RequestStart = new DateTime(2024, 9, 12, 10, 0, 0),
                    RequestEnd = new DateTime(2024, 9, 12, 10, 30, 0)
                }, // 30 minutes
                new Result
                {
                    ResultId = Guid.NewGuid(),
                    ResultSetId = resultSetId,
                    RequestStart = new DateTime(2024, 9, 12, 11, 0, 0),
                    RequestEnd = new DateTime(2024, 9, 12, 11, 15, 0)
                }, // 15 minutes
                new Result
                {
                    ResultId = Guid.NewGuid(),
                    ResultSetId = resultSetId,
                    RequestStart = new DateTime(2024, 9, 12, 12, 0, 0),
                    RequestEnd = new DateTime(2024, 9, 12, 12, 45, 0)
                }  // 45 minutes
            };

            var aicheckerContextMock = new Mock<AicheckerContext>();

            aicheckerContextMock.Setup(x => x.Results).ReturnsDbSet(results);

            // Act 
            var repository = new DefaultMethodesRepository(aicheckerContextMock.Object);
            var result = await repository.ViewAverageTimeOfResultSet(resultSetId);

            // Calculate expected average TimeSpan manually:
            var expectedTicks = (new TimeSpan(0, 30, 0).Ticks + new TimeSpan(0, 15, 0).Ticks + new TimeSpan(0, 45, 0).Ticks) / 3;
            var expectedAverageTimeSpan = TimeSpan.FromTicks(expectedTicks);

            // Assert
            Assert.AreEqual(expectedAverageTimeSpan, result);
        }

    }
}

