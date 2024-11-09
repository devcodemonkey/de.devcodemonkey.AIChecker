using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace de.devcodemonkey.AIChecker.CoreBusiness.Models
{
    public class SendToLmsParams
    {
        public string UserMessage { get; set; }
        public string SystemPrompt { get; set; }
        public string ResultSet { get; set; }
        public int RequestCount { get; set; } = 1;
        public int MaxTokens { get; set; } = -1;
        public double Temperature { get; set; } = 0.7;
        public bool SaveProcessUsage { get; set; } = true;
        public int SaveInterval { get; set; } = 5;
        public bool WriteOutput { get; set; } = true;
        public string? EnvironmentTokenName { get; set; }
        public string Source { get; set; } = "http://localhost:1234/v1/chat/completions";
        public string Model { get; set; } = "nothing set";
        public virtual string? ResponseFormat { get; set; }
    }
}
