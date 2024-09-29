using CommandLine;

namespace de.devcodemonkey.AIChecker.AIChecker.Commands
{
    [Verb("createMoreQuestions", HelpText = "Creates more questions under the 'system prompt' and saves them under the result 'set name'.")]
    public class CreateMoreQuestionsVerb
    {
        [Option('r', "resultSet", Required = true, HelpText = "The result set name.")]
        public string? ResultSet { get; set; }

        [Option('s', "systemPrompt", Required = true, HelpText = "The system prompt or path (path:<file-path>) for creating questions.")]
        public string? SystemPrompt { get; set; }

        [Option('t', "maxTokens", Default = -1, HelpText = "The maximum number of tokens.")]
        public int MaxTokens { get; set; }

        [Option('p', "temperature", Default = 0.7, HelpText = "The temperature.")]
        public double Temperature { get; set; }

        [Option("environmentTokenName", Default = null, HelpText = "The environment token name to set the bearer token for the api.")]
        public string EnvironmentTokenName { get; set; }

        [Option("source", Default = "http://localhost:1234/v1/chat/completions", HelpText = "The source url. Default is http://localhost:1234/v1/chat/completions, default endpoint form Lm Studio.")]
        public string Source { get; set; }

        [Option("model", Default = "nothing set", HelpText = "The model name.")]
        public string Model { get; set; }
    }
}
