using de.devcodemonkey.AIChecker.CoreBusiness.Interfaces;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace de.devcodemonkey.AIChecker.CoreBusiness.Models
{
    public class RequestData
    {
        public string? Model { get; set; }
        public List<IMessage>? Messages { get; set; }
        public double Temperature { get; set; }
        [JsonPropertyName("max_tokens")]
        // for chatgpt
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? MaxTokens { get; set; } = -1;
        public bool Stream { get; set; }
        [JsonIgnore]
        public string Source { get; set; } = "http://localhost:1234/v1/chat/completions";
        [JsonIgnore]
        public string? EnvironmentTokenName { get; set; }
        [JsonIgnore]
        public TimeSpan? RequestTimeout { get; set; }
        [JsonPropertyName("response_format")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]        
        public JsonElement? ResponseFormat { get; set; }
    }

}
