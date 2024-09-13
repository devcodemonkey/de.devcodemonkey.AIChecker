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

        public int CpuUsage { get; set; }
        public DateTime CpuUsageTimestamp { get; set; }

        public int MemoryUsage { get; set; }
        public DateTime MemoryUsageTimestamp { get; set; }        

        public int GpuUsage { get; set; }
        public DateTime GpuUsageTimestamp { get; set; }

        public int GpuTotalMemoryUsage { get; set; }
        public DateTime GpuTotalMemoryUsageTimestamp { get; set; }
    }
}
