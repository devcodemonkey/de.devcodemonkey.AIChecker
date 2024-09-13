namespace de.devcodemonkey.AIChecker.CoreBusiness.DbModels;

public partial class SystemResourceUsage
{
    public Guid SystemResourceUsageId { get; set; }

    public Guid ResultSetId { get; set; }

    public int ProcessId { get; set; }

    public string ProcessName { get; set; } = null!;

    public int CpuUsage { get; set; }

    public DateTime CpuUsageTimestamp { get; set; }

    public int MemoryUsage { get; set; }

    public DateTime MemoryUsageTimestamp { get; set; }

    public int GpuUsage { get; set; }

    public DateTime GpuUsageTimestamp { get; set; }

    public int GpuTotalMemoryUsage { get; set; }

    public DateTime GpuTotalMemoryUsageTimestamp { get; set; }

    public virtual ResultSet ResultSet { get; set; } = null!;
}
