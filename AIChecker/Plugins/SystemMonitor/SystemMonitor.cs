using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Management;

namespace SystemMonitor
{
    [SupportedOSPlatform("windows")]
    public class SystemMonitor
    {
        public async Task<List<ApplicationUsage>> GetApplicationUsagesAsync()
        {
            List<ApplicationUsage> applicationUsages = new List<ApplicationUsage>();

            await Task.Run(() =>
            {
                Thread.CurrentThread.Priority = ThreadPriority.Lowest;

                var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PerfFormattedData_PerfProc_Process");

                foreach (var obj in searcher.Get())
                {
                    var processId = Convert.ToInt32(obj["IDProcess"]);
                    var processName = obj["Name"].ToString();
                    var cpuUsage = Convert.ToDouble(obj["PercentProcessorTime"]);
                    var ramUsage = Convert.ToInt64(obj["WorkingSet"]) / (1024 * 1024);  // RAM usage in MB
                    
                    applicationUsages.Add(new ApplicationUsage
                    {
                        ProcessId = processId,
                        ProcessName = processName,
                        CpuUsage = cpuUsage,
                        RamUsage = ramUsage
                    });
                }
            });

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
