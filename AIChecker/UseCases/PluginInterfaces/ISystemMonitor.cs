using de.devcodemonkey.AIChecker.CoreBusiness.DbModels;
using de.devcodemonkey.AIChecker.CoreBusiness.ModelsSystemMonitor;

namespace de.devcodemonkey.AIChecker.UseCases.PluginInterfaces
{
    public interface ISystemMonitor
    {
        Task<IEnumerable<SystemResourceUsage>> GetApplicationUsagesAsync();
        Task<GpuStatData> GetGpuUsageAsync();
        Task MonitorPerformanceEveryXSecondsAsync(Action<IEnumerable<SystemResourceUsage>> saveAction, int intervalSeconds, CancellationToken cancellationToken, bool writeOutput = false);
    }
}