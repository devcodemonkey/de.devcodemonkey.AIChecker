using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SystemMonitor.Models
{
    public class GpuStatData
    {
        [JsonPropertyName("hostname")]
        public string Hostname { get; set; }

        [JsonPropertyName("driver_version")]
        public string DriverVersion { get; set; }

        [JsonPropertyName("query_time")]
        public DateTime QueryTime { get; set; }

        [JsonPropertyName("gpus")]
        public List<Gpu> Gpus { get; set; }
    }
}
