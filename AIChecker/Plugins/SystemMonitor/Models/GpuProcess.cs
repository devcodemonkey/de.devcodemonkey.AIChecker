using System.Text.Json.Serialization;

namespace SystemMonitor.Models
{
    public class GpuProcess
    {
        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("command")]
        public string Command { get; set; }

        [JsonPropertyName("full_command")]
        public List<string> FullCommand { get; set; }

        [JsonPropertyName("gpu_memory_usage")]
        public long? GpuMemoryUsage { get; set; }

        [JsonPropertyName("cpu_percent")]
        public double CpuPercent { get; set; }

        [JsonPropertyName("cpu_memory_usage")]
        public long CpuMemoryUsage { get; set; }

        [JsonPropertyName("pid")]
        public int Pid { get; set; }
    }
}
