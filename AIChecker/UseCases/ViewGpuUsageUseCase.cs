using de.devcodemonkey.AIChecker.UseCases.Interfaces;
using de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;

public class ViewGpuUsageUseCase : IViewGpuUsageUseCase
{
    private readonly ISystemMonitor _systemMonitor;
    public ViewGpuUsageUseCase(ISystemMonitor systemMonitor) => _systemMonitor = systemMonitor;

    public async Task ExecuteAsync()
    {
        using (var cancellationTokenSource = new CancellationTokenSource())
        {
            await _systemMonitor.MonitorPerformanceEveryXSecondsAsync(async (action) =>
            {
                await Task.Delay(TimeSpan.FromMinutes(1));
            }, 5, cancellationTokenSource.Token, writeOutput: true);
        }
    }
}
