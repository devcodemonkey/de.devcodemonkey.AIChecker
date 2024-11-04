using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace de.devcodemonkey.AIChecker.AIChecker.Commands
{
    [Verb("rankPrompt", HelpText = "Test prompts and create a ranking for it")]
    public class RankPromptVerb
    {
        [Option('r', "resultSet", Required = true, HelpText = "The result set name.")]
        public string ResultSet { get; set; }

        [Option('m', "models", Required = true, Separator = ',', HelpText = "The models to test.")]
        public IEnumerable<string> Models { get; set; }

        [Option('p', "promptRequierements", Required = true, HelpText = "The prompt requierements")]
        public string promptRequierements { get; set; }

        [Option('s', "responseFormat", Required = false, HelpText = "The format of the request. You can set JSON by 'type: \"json_schema\"'")]
        public bool ResponseFormat { get; set; }

        [Option('d', "description", Required = false, HelpText = "The description of the prompt rating.")]
        public string Description { get; set; }

        [Option('t', "maxTokens", Default = -1, HelpText = "The maximum number of tokens.")]
        public int MaxTokens { get; set; } = -1;
    }
}
