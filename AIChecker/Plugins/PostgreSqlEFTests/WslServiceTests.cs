using Microsoft.VisualStudio.TestTools.UnitTesting;
using de.devcodemonkey.AIChecker.DataStore.PostgreSqlEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace de.devcodemonkey.AIChecker.DataStore.PostgreSqlEF.Tests
{
    [TestClass()]
    public class WslServiceTests
    {
        [TestMethod()]
        public void StartDatabaseTest()
        {
            // Arrange
            WslDatabaseService dockerService = new WslDatabaseService();

            // Act
            bool result = dockerService.StartDatabase();

            // Assert
            Assert.IsTrue(result, "The database should start successfully.");
        }

        [TestMethod()]
        public void StopDatabaseTest()
        {
            // Arrange
            WslDatabaseService dockerService = new WslDatabaseService();

            // Act
            bool result = dockerService.StopDatabase();

            // Assert
            Assert.IsTrue(result, "The database should stop successfully.");
        }


        [TestMethod]
        public void RunValidCommandOnWsl_ShouldReturnTrue()
        {
            // Arrange
            string command = "echo HelloWorld";
            WslDatabaseService dockerService = new WslDatabaseService();

            // Act
            bool result = dockerService.RunCommandOnWsl(command);

            // Assert
            Assert.IsTrue(result, "The command should run successfully on WSL.");
        }

        [TestMethod]
        public void RunInvalidCommandOnWsl_ShouldReturnFalse()
        {
            // Arrange
            string invalidCommand = "invalidcommand";
            WslDatabaseService dockerService = new WslDatabaseService();

            // Act
            bool result = dockerService.RunCommandOnWsl(invalidCommand);

            // Assert
            Assert.IsFalse(result, "The command should fail due to invalid input.");
        }
    }
}