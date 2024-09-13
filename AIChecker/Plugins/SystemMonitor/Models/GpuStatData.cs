using System.Text.Json.Serialization;

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
