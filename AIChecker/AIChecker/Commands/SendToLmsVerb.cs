using CommandLine;

namespace de.devcodemonkey.AIChecker.AIChecker.Commands
{
    [Verb("sendToLms", HelpText = "Sends an API request to the LmStudio and saves the result to the db.")]
    public class SendToLmsVerb
    {
        [Option('m', "message", Required = true, HelpText = "The user message.")]
        public string? Message { get; set; }
        [Option('r', "resultSet", Required = true, HelpText = "The result set name.")]
        public string? ResultSet { get; set; }
        [Option('s', "systemPrompt", Default = "You are a helpful assistent", HelpText = "The system prompt.")]
        public string? SystemPrompt { get; set; }
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

        [Option("environmentTokenName", Default = null, HelpText = "The environment token name to set the bearer token for the api.")]
        public string EnvironmentTokenName { get; set; }

        [Option("source", Default = "http://localhost:1234/v1/chat/completions", HelpText = "The source url. Default is http://localhost:1234/v1/chat/completions, default endpoint form Lm Studio.")]
        public string Source { get; set; }

        [Option("model", Default = "nothing set", HelpText = "The model name.")]
        public string Model { get; set; }

    }
}
