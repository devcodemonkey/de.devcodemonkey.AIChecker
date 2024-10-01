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
    public class DockerServiceTests
    {
        [TestMethod]
        public void RunValidCommandOnWsl_ShouldReturnTrue()
        {
            // Arrange
            string command = "echo HelloWorld";
            WslService dockerService = new WslService();

            // Act
            bool result = dockerService.runCommandOnWsl(command);

            // Assert
            Assert.IsTrue(result, "The command should run successfully on WSL.");
        }

        [TestMethod]
        public void RunInvalidCommandOnWsl_ShouldReturnFalse()
        {
            // Arrange
            string invalidCommand = "invalidcommand";
            WslService dockerService = new WslService();

            // Act
            bool result = dockerService.runCommandOnWsl(invalidCommand);

            // Assert
            Assert.IsFalse(result, "The command should fail due to invalid input.");
        }
    }
}