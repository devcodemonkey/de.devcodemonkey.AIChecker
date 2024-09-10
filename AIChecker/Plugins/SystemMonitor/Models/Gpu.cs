using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SystemMonitor.Models
{
    public class Gpu
    {
        [JsonPropertyName("index")]
        public int Index { get; set; }

        [JsonPropertyName("uuid")]
        public string Uuid { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("temperature.gpu")]
        public int TemperatureGpu { get; set; }

        [JsonPropertyName("fan.speed")]
        public int? FanSpeed { get; set; }

        [JsonPropertyName("utilization.gpu")]
        public int UtilizationGpu { get; set; }

        [JsonPropertyName("utilization.enc")]
        public int UtilizationEnc { get; set; }

        [JsonPropertyName("utilization.dec")]
        public int UtilizationDec { get; set; }

        [JsonPropertyName("power.draw")]
        public int PowerDraw { get; set; }

        [JsonPropertyName("enforced.power.limit")]
        public int EnforcedPowerLimit { get; set; }

        [JsonPropertyName("memory.used")]
        public int MemoryUsed { get; set; }

        [JsonPropertyName("memory.total")]
        public int MemoryTotal { get; set; }

        [JsonPropertyName("processes")]
        public List<GpuProcess> Processes { get; set; }
    }
}
