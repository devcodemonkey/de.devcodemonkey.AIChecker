using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace de.devcodemonkey.AIChecker.CoreBusiness.Models
{
    public class GpuMemoryUsage
    {
        public int ProcessId { get; set; }
        public long DedicatedUsage { get; set; }
        public long LocalUsage { get; set; }
        public long NonLocalUsage { get; set; }
        public long SharedUsage { get; set; }
        public long TotalUsage { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
