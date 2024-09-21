using de.devcodemonkey.AIChecker.CoreBusiness.DbModels;
using de.devcodemonkey.AIChecker.CoreBusiness.ModelsSystemMonitor;
using de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;
using Spectre.Console;
using System.Diagnostics;
using System.Management;
using System.Runtime.Versioning;
using System.Text.Json;


namespace de.devcodemonkey.AIChecker.DataSource.SystemMonitor
{
    [SupportedOSPlatform("windows")]
    public class SystemMonitor : ISystemMonitor
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
                    var cpuUsage = Convert.ToInt32(obj["PercentProcessorTime"]);
                    var memoryUsage = Convert.ToInt32(Convert.ToInt64(obj["WorkingSet"]) / (1024 * 1024));  // RAM usage in MB                   

                    var gpuUsage = Convert.ToInt32((gpuStat.Gpus.FirstOrDefault()
                                .Processes
                                .Where(p => p.Pid == processId)
                                .FirstOrDefault()?.CpuPercent ?? 0));

                    var gpuTotalMemoryUsage = gpuStat.Gpus.FirstOrDefault()
                                .MemoryUsed;


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
                        GpuTotalMemoryUsageTimestamp = gpuStat.QueryTime,
                        GpuTotalMemoryUsage = gpuTotalMemoryUsage,
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
                    //Console.WriteLine("Current Performance Data:");
                    //foreach (var usage in usageList)
                    //{
                    //    Console.WriteLine(
                    //       $"Process: {usage.ProcessName}, " +
                    //       $"CPU: {usage.CpuUsage:F2}% at {usage.CpuUsageTimestamp}, " +
                    //       $"RAM: {usage.MemoryUsage}MB at {usage.MemoryUsageTimestamp}, " +
                    //       $"GPU: {usage.GpuUsage}MB at {usage.GpuUsageTimestamp}"
                    //   );
                    //}
                    //Console.WriteLine("----------------------------------------");
                    var topGpuUsage = usageList
                                        .Where(p => p.ProcessName != "_Total")
                                        .OrderByDescending(u => u.MemoryUsage)
                                        .OrderByDescending(u => u.CpuUsage)
                                        .OrderByDescending(u => u.GpuUsage)
                                        .Take(10);

                    AnsiConsole.Write(new Rule("[yellow]System Resource Usage[/]").RuleStyle("green"));                   

                    AnsiConsole.MarkupLine($"Time: {DateTime.Now}, Used GPU Memory: {topGpuUsage.FirstOrDefault()?.GpuTotalMemoryUsage ?? 0} MB");


                    // Create a table
                    var table = new Table();

                    //table.Title("System Resource Usage");

                    // Add columns
                    table.AddColumn("[bold yellow]No.[/]");
                    table.AddColumn("[bold yellow]Process[/]");
                    table.AddColumn(new TableColumn("[bold yellow]CPU Usage[/]").RightAligned());
                    table.AddColumn("[bold yellow]CPU Timestamp[/]");
                    table.AddColumn(new TableColumn("[bold yellow]RAM Usage[/]").RightAligned());
                    table.AddColumn("[bold yellow]RAM Timestamp[/]");
                    table.AddColumn(new TableColumn("[bold yellow]GPU Usage[/]").RightAligned());
                    table.AddColumn("[bold yellow]GPU Timestamp[/]");

                    foreach (var (usage, index) in topGpuUsage.Select((usage, index) => (usage, index)))
                    {
                        table.AddRow(
                            (index + 1).ToString(),
                            $"[bold]{usage.ProcessName}[/]",  // Highlight process name
                            $"[green]{usage.CpuUsage / 100:F2}%[/]",  // Display CPU usage with formatting
                            usage.CpuUsageTimestamp.ToString("yyyy-MM-dd HH:mm:ss"),  // Format timestamps
                            $"[blue]{usage.MemoryUsage} MB[/]",  // Display RAM usage in MB
                            usage.MemoryUsageTimestamp.ToString("yyyy-MM-dd HH:mm:ss"),
                            $"[red]{usage.GpuUsage:F2}%[/]",  // Display GPU usage with a different color
                            usage.GpuUsageTimestamp.ToString("yyyy-MM-dd HH:mm:ss")
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
    }
}
