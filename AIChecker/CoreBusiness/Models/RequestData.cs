using de.devcodemonkey.AIChecker.CoreBusiness.Interfaces;
using System.Text.Json.Serialization;

namespace de.devcodemonkey.AIChecker.CoreBusiness.Models
{
    public class RequestData
    {
        public string? Model { get; set; }
        public List<IMessage>? Messages { get; set; }
        public double Temperature { get; set; }
        [JsonPropertyName("max_tokens")]
        public int? MaxTokens { get; set; }
        public bool Stream { get; set; }

        public string Source { get; set; } = "http://localhost:1234/v1/chat/completions";
        public string? EnvironmentTokenName { get; set; }
        public TimeSpan? RequestTimeout { get; set; }
    }
}
