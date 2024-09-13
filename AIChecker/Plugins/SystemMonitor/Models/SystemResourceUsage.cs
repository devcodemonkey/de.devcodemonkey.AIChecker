using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemMonitor.Models
{
    public class SystemResourceUsage
    {
        public int ProcessId { get; set; }

        public string ProcessName { get; set; }

        public double CpuUsage { get; set; }
        public DateTime CpuUsageTimestamp { get; set; }

        public long MemoryUsage { get; set; }
        public DateTime MemoryUsageTimestamp { get; set; }


        public long GpuMemory { get; set; }
        public DateTime GpuMemoryTimestamp { get; set; }

        public double GpuUsage { get; set; }
        public DateTime GpuUsageTimestamp { get; set; }
    }
}
