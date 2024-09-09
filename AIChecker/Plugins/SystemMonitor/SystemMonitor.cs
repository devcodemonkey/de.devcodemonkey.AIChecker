using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace SystemMonitor
{
    [SupportedOSPlatform("windows")]
    public class SystemMonitor
    {        
        public async Task<List<ApplicationUsage>> GetApplicationUsagesAsync()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Console.WriteLine("Application usage is only supported on Windows platforms.");
                return null;
            }

            List<ApplicationUsage> applicationUsages = new List<ApplicationUsage>();
            var cpuCounters = new Dictionary<int, PerformanceCounter>();

            foreach (Process process in Process.GetProcesses())
            {
                // Initialize CPU performance counter for the process if it doesn't exist
                if (!cpuCounters.ContainsKey(process.Id))
                {
                    cpuCounters[process.Id] = new PerformanceCounter("Process", "% Processor Time", process.ProcessName, true);
                }

                // Get CPU usage asynchronously
                var cpuUsage = await Task.Run(() =>
                {
                    // Wait a moment to allow PerformanceCounter to calculate CPU usage
                    cpuCounters[process.Id].NextValue();
                    Task.Delay(500).Wait(); // Delay asynchronously
                    return cpuCounters[process.Id].NextValue() / Environment.ProcessorCount;
                });

                // Add process information to the list
                applicationUsages.Add(new ApplicationUsage
                {
                    ProcessId = process.Id,
                    ProcessName = process.ProcessName,
                    CpuUsage = cpuUsage,
                    RamUsage = process.WorkingSet64 / (1024 * 1024),  // RAM usage in MB
                    GpuUsage = process.WorkingSet64 / (1024 * 1024)   // Substitute for GPU, using RAM usage
                });
            }

            return applicationUsages;
        }

        public async Task<List<ApplicationUsage>> MonitorPerformanceEveryXSecondsAsync(int intervalSeconds, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var usageList = await GetApplicationUsagesAsync();

                if (usageList != null)
                {
                    Console.WriteLine("Current Performance Data:");
                    foreach (var usage in usageList)
                    {
                        Console.WriteLine($"Process: {usage.ProcessName}, CPU: {usage.CpuUsage:F2}%, RAM: {usage.RamUsage}MB");
                    }
                    Console.WriteLine("----------------------------------------");

                    // Return the usage list to be saved by the caller
                    return usageList;
                }

                // Wait for the specified interval or stop if cancellation is requested
                try
                {
                    await Task.Delay(intervalSeconds * 1000, cancellationToken);
                }
                catch (TaskCanceledException)
                {
                    break; // Break out of the loop if cancellation is requested
                }
            }

            Console.WriteLine("Monitoring stopped.");
            return null; // In case the loop ends without new data
        }
    }
}
