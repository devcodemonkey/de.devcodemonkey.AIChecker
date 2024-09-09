using Microsoft.VisualStudio.TestTools.UnitTesting;
using SystemMonitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SystemMonitor.Tests
{
    [TestClass()]
    public class SystemMonitorTests
    {
        [TestMethod]
        public async Task GetApplicationUsagesAsync_ReturnsNonEmptyList()
        {
            // Arrange
            var monitor = new SystemMonitor();

            // Act
            var applicationUsages = await monitor.GetApplicationUsagesAsync();

            //write all properties to trace output in one line
            foreach (var item in applicationUsages.OrderByDescending(c=>c.gpu))
            {
                Debug.WriteLine($"Process ID: {item.ProcessId}, Process Name: {item.ProcessName}, CPU Usage: {item.CpuUsage}, RAM Usage: {item.RamUsage}, GPU Usage: {item.GpuUsage}");
            }



            // Assert
            Assert.IsNotNull(applicationUsages, "The list of application usages should not be null.");
            Assert.IsTrue(applicationUsages.Count > 0, "The list of application usages should contain at least one process.");

            // Further assert that the processes have valid data
            var firstProcess = applicationUsages.FirstOrDefault();
            Assert.IsNotNull(firstProcess, "At least one process should exist.");

            Assert.IsTrue(firstProcess.ProcessId >= 0, "Process ID should be greater than 0.");
            Assert.IsFalse(string.IsNullOrWhiteSpace(firstProcess.ProcessName), "Process Name should not be null or empty.");
            Assert.IsTrue(firstProcess.CpuUsage >= 0, "CPU usage should be 0 or greater.");
            Assert.IsTrue(firstProcess.RamUsage >= 0, "RAM usage should be 0 or greater.");
        }



        [TestMethod()]
        public async Task MonitorPerformanceEveryXSecondsAsyncTest()
        {

        }

        
    }
}