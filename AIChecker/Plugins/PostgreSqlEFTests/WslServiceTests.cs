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

            dockerService.StopDatabase();
        }

        [TestMethod()]
        public void StopDatabaseTest()
        {
            // Arrange
            WslDatabaseService dockerService = new WslDatabaseService();
            dockerService.StartDatabase();

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

        [TestMethod()]
        public void BackupDatabaseToGitTest()
        {
            // Arrange
            WslDatabaseService dockerService = new WslDatabaseService();
            dockerService.StartDatabase();
            // sleep for 10 seconds to allow the database to start
            Thread.Sleep(10000);            

            // Act
            bool result = dockerService.BackupDatabaseToGit("ssh://git@gitlab.hl-dev.de:39566/aichecker", "de.devcocdemonkey.aichecker.dbbackup");

            // Assert
            Assert.IsTrue(result, "The database should be backed up to Git successfully.");

            dockerService.StopDatabase();
        }
    }
}