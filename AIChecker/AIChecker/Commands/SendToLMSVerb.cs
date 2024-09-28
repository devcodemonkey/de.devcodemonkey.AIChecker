using CommandLine;

namespace de.devcodemonkey.AIChecker.AIChecker.Commands
{
    [Verb("sendToLms", HelpText = "Sends an API request to the LmStudio and saves the result to the db.")]
    public class SendToLmsVerb
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
        [Option('u', "saveProcessUsage", HelpText = "Save the process usage. Default is false.")]
        public bool SaveProcessUsage { get; set; }

        [Option('i', "saveInterval", Default = 5, HelpText = "The save interval.")]
        public int SaveInterval { get; set; }
        [Option('w', "writeOutput", Default = true, HelpText = "Write process output to console")]
        public bool WriteOutput { get; set; }
    }
}
