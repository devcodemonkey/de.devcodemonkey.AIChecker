using CommandLine;
using de.devcodemonkey.AIChecker.CoreBusiness.Models;

namespace de.devcodemonkey.AIChecker.AIChecker.Commands
{
    [Verb("sendToLms", HelpText = "Sends an API request to the LmStudio and saves the result to the db.")]
    public class SendToLmsVerb : SendToLmsParams
    {
        [Option('m', "message", Required = false, HelpText = "The user message. Required if questionCategory is not set.")]
        public override string UserMessage { get; set; }
        [Option('r', "resultSet", Required = true, HelpText = "The result set name.")]
        public override string ResultSet { get; set; }
        [Option('s', "systemPrompt", Default = "You are a helpful assistent", HelpText = "The system prompt. (Default: You are a helpful assisten) If you want to ask for it, set an 'ask' parameter.")]
        public override string SystemPrompt { get; set; }
        [Option('c', "requestCount", Default = 1, HelpText = "The number of requests.")]
        public override int RequestCount { get; set; }
        [Option('t', "maxTokens", Default = -1, HelpText = "The maximum number of tokens.")]
        public override int MaxTokens { get; set; }
        [Option('p', "temperature", Default = 0.7, HelpText = "The temperature.")]
        public override double Temperature { get; set; }
        [Option('u', "saveProcessUsage", HelpText = "Save the process usage. Default is false.")]
        public override bool SaveProcessUsage { get; set; }

        [Option('i', "saveInterval", Default = 5, HelpText = "The save interval.")]
        public override int SaveInterval { get; set; }
        [Option('w', "writeOutput", Default = true, HelpText = "Write process output to console")]
        public override bool WriteOutput { get; set; }

        [Option("environmentTokenName", Default = null, HelpText = "The environment token name to set the bearer token for the api.")]
        public override string EnvironmentTokenName { get; set; }

        [Option("source", Default = "http://localhost:1234/v1/chat/completions", HelpText = "The source url. Default is http://localhost:1234/v1/chat/completions, default endpoint form Lm Studio.")]
        public override string Source { get; set; }

        [Option("model", Default = "nothing set", HelpText = "The model name.")]
        public override string Model { get; set; }

        [Option("responseFormat", Default = null, HelpText = "The response format. If you want to ask for it, set an 'ask' parameter.")]
        public override string ResponseFormat { get; set; }

        [Option("questionCategory", Default = null, HelpText = "The question category.")]
        public override string QuestionCategory { get; set; }

    }
}
