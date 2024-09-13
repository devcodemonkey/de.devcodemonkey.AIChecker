using CommandLine;

namespace de.devcodemonkey.AIChecker.AIChecker.Commands
{
    [Verb("sendToLMS", HelpText = "Sends an API request to the LmStudio and saves the result to the db.")]
    public class SendToLMSVerb
    {
        [Option('m', "message", Required = true, HelpText = "The user message.")]
        public string? Message { get; set; }
        [Option('s', "systemPrompt", Required = true, HelpText = "The system prompt.")]
        public string? SystemPrompt { get; set; }
        [Option('r', "resultSet", Required = true, HelpText = "The result set name.")]
        public string? ResultSet { get; set; }
        [Option('c', "requestCount", Default = 1, HelpText = "The number of requests.")]
        public int RequestCount { get; set; }
        [Option('t', "maxTokens", Default = -1, HelpText = "The maximum number of tokens.")]
        public int MaxTokens { get; set; }
        [Option('p', "temperature", Default = 0.7, HelpText = "The temperature.")]
        public double Temperature { get; set; }
        [Option('u', "saveProcessUsage", Default = true, HelpText = "Whether to save the process usage.")]
        public bool SaveProcessUsage { get; set; }
        [Option('i', "saveInterval", Default = 5, HelpText = "The save interval.")]
        public int SaveInterval { get; set; }
    }
}
