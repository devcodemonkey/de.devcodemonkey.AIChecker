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

        
        
    }
}
