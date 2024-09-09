using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemMonitor
{
    public class ApplicationUsage
    {
        public int ProcessId { get; set; }

        public string ProcessName { get; set; }

        public long GpuUsage { get; set; }
        public float CpuUsage { get; internal set; }
        public long RamUsage { get; internal set; }
    }
}
