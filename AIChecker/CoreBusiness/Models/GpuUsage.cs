using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace de.devcodemonkey.AIChecker.CoreBusiness.Models
{
    public class GpuProcessUsage
    {
        public int ProcessId { get; set; }
        public int GpuUsage { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
