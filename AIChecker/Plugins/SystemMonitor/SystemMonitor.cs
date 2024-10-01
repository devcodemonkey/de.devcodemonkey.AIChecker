using de.devcodemonkey.AIChecker.CoreBusiness.DbModels;
using de.devcodemonkey.AIChecker.CoreBusiness.Models;
using de.devcodemonkey.AIChecker.CoreBusiness.ModelsSystemMonitor;
using de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;
using Spectre.Console;
using System.Diagnostics;
using System.Management;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text.Json;
using System.Text.RegularExpressions;


namespace de.devcodemonkey.AIChecker.DataSource.SystemMonitor
{    
    public class SystemMonitor : ISystemMonitor
    {
        public async Task<IEnumerable<SystemResourceUsage>> GetApplicationUsagesAsync()
        {
            List<SystemResourceUsage> applicationUsages = new List<SystemResourceUsage>();

            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                AnsiConsole.MarkupLine("[red]This method is only supported on Windows. Returning dummy data.[/]");

                for (int i = 0; i < 10; i++)
                {
                    applicationUsages.Add(new SystemResourceUsage
                    {
                        ProcessId = i,
                        ProcessName = $"Process {i}",
                        CpuUsage = new Random().Next(0, 10000),
                        CpuUsageTimestamp = DateTime.Now,
                        MemoryUsage = new Random().Next(0, 1000),
                        MemoryUsageTimestamp = DateTime.Now,
                        GpuUsage = new Random().Next(0, 100),
                        GpuUsageTimestamp = DateTime.Now,
                        GpuMemoryUsage = new Random().Next(0, 1000),
                        GpuMemoryUsageTimestamp = DateTime.Now,
                    });
                }

                return applicationUsages;
            }


            var gpuMemoryUsage = GetGpuMemoryUsages();
            var gpuProcessUsage = GetGpuProcessUsage();

            await Task.Run(() =>
            {
                Thread.CurrentThread.Priority = ThreadPriority.Lowest;

                var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PerfFormattedData_PerfProc_Process");
                var cpuGpuTimestamp = DateTime.Now;

                foreach (var obj in searcher.Get())
                {
                    var processId = Convert.ToInt32(obj["IDProcess"]);
                    var processName = obj["Name"].ToString();
                    var cpuUsage = Convert.ToInt32(obj["PercentProcessorTime"]);
                    var memoryUsage = Convert.ToInt32(Convert.ToInt64(obj["WorkingSet"]) / (1024 * 1024));  // RAM usage in MB                   

                    var gpuProcessUsageTmp = gpuProcessUsage
                                                .Where(p => p.ProcessId == processId)
                                                .FirstOrDefault();

                    var gpuMemoryUsageTmp = gpuMemoryUsage
                                    .Where(p => p.ProcessId == processId)
                                    .FirstOrDefault();

                    var systeResourceUsage = new SystemResourceUsage
                    {
                        ProcessId = processId,
                        ProcessName = processName,

                        CpuUsage = cpuUsage,
                        CpuUsageTimestamp = cpuGpuTimestamp,
                        MemoryUsage = memoryUsage,
                        MemoryUsageTimestamp = cpuGpuTimestamp,

                        GpuUsage = gpuProcessUsageTmp?.GpuUsage ?? 0,
                        GpuMemoryUsage = Convert.ToInt32(gpuMemoryUsageTmp?.TotalUsage / (1024 * 1024)), // in MB                        
                    };

                    if (gpuProcessUsageTmp != null)
                        systeResourceUsage.GpuUsageTimestamp = gpuProcessUsageTmp.Timestamp;
                    if (gpuMemoryUsageTmp != null)
                        systeResourceUsage.GpuMemoryUsageTimestamp = gpuMemoryUsageTmp.Timestamp;

                    applicationUsages.Add(systeResourceUsage);
                }
            });

            return applicationUsages;
        }

        public List<GpuProcessUsage> GetGpuProcessUsage()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                AnsiConsole.MarkupLine("[red]This method is only supported on Windows.[/]");
                return null;
            }
            var searcher = new ManagementObjectSearcher("select * from Win32_PerfFormattedData_GPUPerformanceCounters_GPUEngine");

            var processUsageList = new List<GpuProcessUsage>();
            foreach (var obj in searcher.Get())
            {
                string pid = obj["Name"].ToString();

                if (GetProcessIdFromPid(pid, out int pidInt))
                {
                    var gpuUsage = Convert.ToInt32(obj["UtilizationPercentage"]);
                    var gpuProcessUsage = new GpuProcessUsage
                    {
                        ProcessId = pidInt,
                        GpuUsage = gpuUsage,
                        Timestamp = DateTime.Now
                    };
                    processUsageList.Add(gpuProcessUsage);
                }
            }
            return processUsageList;
        }

        public List<GpuMemoryUsage> GetGpuMemoryUsages()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                AnsiConsole.MarkupLine("[red]This method is only supported on Windows.[/]");
                return null;
            }
            var searcher = new ManagementObjectSearcher("select * from Win32_PerfFormattedData_GPUPerformanceCounters_GPUProcessMemory");

            var gpuMemoryUsageList = new List<GpuMemoryUsage>();
            foreach (var obj in searcher.Get())
            {
                string pid = obj["Name"].ToString();

                if (GetProcessIdFromPid(pid, out int pidInt))
                {
                    var dedicatedUsage = Convert.ToInt64(obj["DedicatedUsage"]);
                    var localUsage = Convert.ToInt64(obj["LocalUsage"]);
                    var nonLocalUsage = Convert.ToInt64(obj["NonLocalUsage"]);
                    var sharedUsage = Convert.ToInt64(obj["SharedUsage"]);
                    var totalUsage = Convert.ToInt64(obj["TotalCommitted"]);

                    var gpuMemoryUsage = new GpuMemoryUsage
                    {
                        ProcessId = pidInt,
                        LocalUsage = localUsage,
                        NonLocalUsage = nonLocalUsage,
                        SharedUsage = sharedUsage,
                        TotalUsage = totalUsage,
                        Timestamp = DateTime.Now,
                    };
                    gpuMemoryUsageList.Add(gpuMemoryUsage);
                }
            }
            return gpuMemoryUsageList;
        }

        [Obsolete]
        public async Task<GpuStatData> GetGpuUsageAsync()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                AnsiConsole.MarkupLine("[red]This method is only supported on Windows.[/]");
                return null;
            }

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

                    //Console.WriteLine(result);
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
                    var topGpuUsage = usageList
                                        .Where(p => p.ProcessName != "_Total")
                                        .OrderByDescending(u => u.MemoryUsage)
                                        .OrderByDescending(u => u.CpuUsage)
                                        .OrderByDescending(u => u.GpuMemoryUsage)
                                        .OrderByDescending(u => u.GpuUsage)
                                        .Take(10);

                    AnsiConsole.Write(new Rule("[yellow]System Resource Usage[/]").RuleStyle("green"));

                    //AnsiConsole.MarkupLine($"Time: {DateTime.Now}, Used GPU Memory: {topGpuUsage.FirstOrDefault()?.GpuTotalMemoryUsage ?? 0} MB");

                    // Create a table
                    var table = new Table();

                    // Add columns
                    table.AddColumn("[bold yellow]No.[/]");
                    table.AddColumn("[bold yellow]Process[/]");

                    table.AddColumn(new TableColumn("[bold yellow]GPU Usage[/]").RightAligned());
                    table.AddColumn("[bold yellow]GPU Timestamp[/]");

                    table.AddColumn(new TableColumn("[bold yellow]GPU Memory Usage[/]").RightAligned());
                    table.AddColumn("[bold yellow]GPU Memory Timestamp[/]");

                    table.AddColumn(new TableColumn("[bold yellow]CPU Usage[/]").RightAligned());
                    table.AddColumn("[bold yellow]CPU Timestamp[/]");

                    table.AddColumn(new TableColumn("[bold yellow]RAM Usage[/]").RightAligned());
                    table.AddColumn("[bold yellow]RAM Timestamp[/]");

                    

                    foreach (var (usage, index) in topGpuUsage.Select((usage, index) => (usage, index)))
                    {
                        table.AddRow(
                            (index + 1).ToString(),
                            $"[bold]{usage.ProcessName}[/]",  // Highlight process name

                            $"[red]{usage.GpuUsage:F2}%[/]",  // Display GPU usage with a different color
                            usage.GpuUsageTimestamp.ToString("yyyy-MM-dd HH:mm:ss"),

                            $"[darkorange]{usage.GpuMemoryUsage} MB[/]",  // Display GPU memory usage in MB
                            usage.GpuMemoryUsageTimestamp.ToString("yyyy-MM-dd HH:mm:ss"),
                            
                            $"[green]{usage.CpuUsage / 100:F2}%[/]",  // Display CPU usage with formatting
                            usage.CpuUsageTimestamp.ToString("yyyy-MM-dd HH:mm:ss"),  // Format timestamps

                            $"[blue]{usage.MemoryUsage} MB[/]",  // Display RAM usage in MB
                            usage.MemoryUsageTimestamp.ToString("yyyy-MM-dd HH:mm:ss")                            
                        );
                    }

                    AnsiConsole.Write(table);
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

        private bool GetProcessIdFromPid(string input, out int pid)
        {
            pid = -1;
            string pattern = @"pid_(\d+)";
            var match = Regex.Match(input, pattern);
            if (match.Success)
            {
                var pidTmp = match.Groups[1].Value;
                if (int.TryParse(pidTmp, out pid))
                    return true;
            }
            return false;
        }
    }
}
