using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Management;
using System.Collections.Generic;
using System.Text.Json;
using SystemMonitor.Models;

namespace de.devcodemonkey.AIChecker.DataSource.SystemMonitor
{
    [SupportedOSPlatform("windows")]
    public class SystemMonitor
    {
        public async Task<IEnumerable<ApplicationUsage>> GetApplicationUsagesAsync()
        {
            List<ApplicationUsage> applicationUsages = new List<ApplicationUsage>();

            var gpuStat = await GetGpuUsageAsync();

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
                    var gpuUsage = gpuStat.Gpus.FirstOrDefault()
                                .Processes
                                .Where(p => p.Pid == processId)
                                .FirstOrDefault()?.CpuMemoryUsage ?? 0;


                    applicationUsages.Add(new ApplicationUsage
                    {
                        ProcessId = processId,
                        ProcessName = processName,
                        CpuUsage = cpuUsage,
                        RamUsage = ramUsage,
                        GpuUsage = gpuUsage
                    });
                }
            });

            return applicationUsages;
        }

        public async Task<GpuStatData> GetGpuUsageAsync()
        {
            ProcessStartInfo start = new ProcessStartInfo
            {
                FileName = Path.Combine("PythonGpuStat", "dist", "gpustat.exe"),  // Full path to the standalone gpustat executable
                Arguments = "--show-full-cmd --json",  // Add arguments if needed
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            GpuStatData gpuStat = null;

            using (Process process = new Process { StartInfo = start })
            {
                // Start the process asynchronously
                process.Start();

                // Read the StandardOutput asynchronously
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = await reader.ReadToEndAsync();

                    // Deserialize the JSON result into the GpuStatData class
                    gpuStat = JsonSerializer.Deserialize<GpuStatData>(result);

                    Console.WriteLine(result);
                }

                // Read the StandardError asynchronously
                using (StreamReader reader = process.StandardError)
                {
                    string error = await reader.ReadToEndAsync();
                    if (!string.IsNullOrEmpty(error))
                    {
                        Console.WriteLine($"Error: {error}");
                    }
                }

                // Wait for the process to exit
                await process.WaitForExitAsync();
            }

            return gpuStat;
        }


        public async Task MonitorPerformanceEveryXSecondsAsync(Action<IEnumerable<ApplicationUsage>> saveAction, int intervalSeconds, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var usageList = await GetApplicationUsagesAsync();

                saveAction(usageList);

                if (usageList != null)
                {
                    Console.WriteLine("Current Performance Data:");
                    foreach (var usage in usageList)
                    {
                        Console.WriteLine($"Process: {usage.ProcessName}, CPU: {usage.CpuUsage:F2}%, RAM: {usage.RamUsage}MB");
                    }
                    Console.WriteLine("----------------------------------------");                
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
        }
    }
}
