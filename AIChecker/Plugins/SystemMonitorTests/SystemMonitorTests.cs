using Microsoft.VisualStudio.TestTools.UnitTesting;
using SystemMonitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.Versioning;
using System.Threading;
using SystemMonitor.Models;
using de.devcodemonkey.AIChecker.CoreBusiness.DbModels;

namespace de.devcodemonkey.AIChecker.DataSource.SystemMonitor.Tests
{
    [SupportedOSPlatform("windows")]
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
            foreach (var item in applicationUsages
                                    .OrderByDescending(g => g.GpuTotalMemoryUsage)
                                    .ThenByDescending(g => g.GpuUsage)
                                    .ThenByDescending(c => c.CpuUsage)
                                    .ThenByDescending(r => r.MemoryUsage))
            {
                Debug.WriteLine($"Process ID: {item.ProcessId}, Process Name: {item.ProcessName}, CPU Usage: {item.CpuUsage}, RAM Usage: {item.MemoryUsage}, GPU Usage: {item.GpuUsage}, GPU TotalMemory {item.GpuTotalMemoryUsage}");
            }



            // Assert
            Assert.IsNotNull(applicationUsages, "The list of application usages should not be null.");
            Assert.IsTrue(applicationUsages.Count() > 0, "The list of application usages should contain at least one process.");

            // Further assert that the processes have valid data
            var firstProcess = applicationUsages.FirstOrDefault();
            Assert.IsNotNull(firstProcess, "At least one process should exist.");

            Assert.IsTrue(firstProcess.ProcessId >= 0, "Process ID should be greater than 0.");
            Assert.IsFalse(string.IsNullOrWhiteSpace(firstProcess.ProcessName), "Process Name should not be null or empty.");
            Assert.IsTrue(firstProcess.CpuUsage >= 0, "CPU usage should be 0 or greater.");
            Assert.IsTrue(firstProcess.MemoryUsage >= 0, "RAM usage should be 0 or greater.");

            Assert.IsTrue(firstProcess.GpuUsage >= 0, "GPU usage should be 0 or greater.");
            Assert.IsTrue(firstProcess.GpuTotalMemoryUsage >= 0, "GPU memory usage should be 0 or greater.");
        }



        [TestMethod()]
        public async Task MonitorPerformanceEveryXSecondsAsyncTest()
        {
            // Arrange
            var monitor = new SystemMonitor();

            // Act
            var allUsageData = new List<IEnumerable<SystemResourceUsage>>();

            // Create a cancellation token source to control the task
            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                // Start the monitoring task
                var monitoringTask = monitor.MonitorPerformanceEveryXSecondsAsync((applicationUsages) =>
                {
                    allUsageData.Add(applicationUsages);
                }, 5, cancellationTokenSource.Token);

                // example process to monitor for 20 seconds
                await Task.Delay(23000);

                cancellationTokenSource.Cancel();

                // Await the monitoring task to handle final cleanup and stop gracefully
                await monitoringTask;
            }

            // Assert                       
            Assert.IsTrue(allUsageData.Count <= 4 && allUsageData.Count >= 3, "The number of tracked prcosesses should be between 3 and 4.");
        }

        [TestMethod()]
        public async Task GetGpuUsageTest()
        {
            // Arrange
            var monitor = new SystemMonitor();

            // Act
            var gpuUsage = await monitor.GetGpuUsageAsync();

            // Assert
        }
    }
}