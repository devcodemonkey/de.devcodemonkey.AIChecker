﻿using System.Diagnostics;
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
        public async Task<IEnumerable<SystemResourceUsage>> GetApplicationUsagesAsync()
        {
            List<SystemResourceUsage> applicationUsages = new List<SystemResourceUsage>();

            var gpuStat = await GetGpuUsageAsync();

            await Task.Run(() =>
            {
                Thread.CurrentThread.Priority = ThreadPriority.Lowest;

                var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PerfFormattedData_PerfProc_Process");
                var cpuGpuTimestamp = DateTime.Now;

                foreach (var obj in searcher.Get())
                {
                    var processId = Convert.ToInt32(obj["IDProcess"]);
                    var processName = obj["Name"].ToString();
                    var cpuUsage = Convert.ToDouble(obj["PercentProcessorTime"]);
                    var memoryUsage = Convert.ToInt64(obj["WorkingSet"]) / (1024 * 1024);  // RAM usage in MB
                    var gpuMemory = gpuStat.Gpus.FirstOrDefault()
                                .Processes
                                .Where(p => p.Pid == processId)
                                .FirstOrDefault()?.CpuMemoryUsage ?? 0;
                    gpuMemory = gpuMemory / 1024;  // GPU usage in MB

                    var gpuUsage = gpuStat.Gpus.FirstOrDefault()
                                .Processes
                                .Where(p => p.Pid == processId)
                                .FirstOrDefault()?.CpuPercent ?? 0;

                    applicationUsages.Add(new SystemResourceUsage
                    {
                        ProcessId = processId,
                        ProcessName = processName,

                        CpuUsage = cpuUsage,
                        CpuUsageTimestamp = cpuGpuTimestamp,
                        MemoryUsage = memoryUsage,
                        MemoryUsageTimestamp = cpuGpuTimestamp,

                        GpuUsage = gpuUsage,
                        GpuUsageTimestamp = gpuStat.QueryTime,
                        GpuMemoryTimestamp = gpuStat.QueryTime,
                        GpuMemory = gpuMemory,                        
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


        public async Task MonitorPerformanceEveryXSecondsAsync(
            Action<IEnumerable<SystemResourceUsage>> saveAction,
            int intervalSeconds,
            CancellationToken cancellationToken,
            bool writeOutput = false)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var usageList = await GetApplicationUsagesAsync();

                saveAction(usageList);

                if (usageList != null && writeOutput)
                {
                    Console.WriteLine("Current Performance Data:");
                    foreach (var usage in usageList)
                    {
                        Console.WriteLine(
                           $"Process: {usage.ProcessName}, " +
                           $"CPU: {usage.CpuUsage:F2}% at {usage.CpuUsageTimestamp}, " +
                           $"RAM: {usage.MemoryUsage}MB at {usage.MemoryUsageTimestamp}, " +
                           $"GPU: {usage.GpuUsage}MB at {usage.GpuUsageTimestamp}"
                       );
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
