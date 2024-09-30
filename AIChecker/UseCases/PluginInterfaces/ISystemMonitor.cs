using de.devcodemonkey.AIChecker.CoreBusiness.DbModels;
using de.devcodemonkey.AIChecker.CoreBusiness.Models;
using de.devcodemonkey.AIChecker.CoreBusiness.ModelsSystemMonitor;

namespace de.devcodemonkey.AIChecker.UseCases.PluginInterfaces
{
    public interface ISystemMonitor
    {
        Task<IEnumerable<SystemResourceUsage>> GetApplicationUsagesAsync();
        List<GpuMemoryUsage> GetGpuMemoryUsages();
        Task<GpuStatData> GetGpuUsageAsync();
        List<GpuProcessUsage> GetProcessUsage();
        Task MonitorPerformanceEveryXSecondsAsync(Action<IEnumerable<SystemResourceUsage>> saveAction, int intervalSeconds, CancellationToken cancellationToken, bool writeOutput = false);
    }
}