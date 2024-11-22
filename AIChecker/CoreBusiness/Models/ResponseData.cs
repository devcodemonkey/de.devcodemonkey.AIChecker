using System.Text.Json.Serialization;

namespace de.devcodemonkey.AIChecker.CoreBusiness.Models
{
    public class ResponseData
    {
        public string? Id { get; set; }
        public string? Object { get; set; }
        public long? Created { get; set; }
        public string? Model { get; set; }
        public List<Choice>? Choices { get; set; }
        public Usage? Usage { get; set; }
        [JsonPropertyName("system_fingerprint")]
        public string? SystemFingerprint { get; set; }
    }
}
