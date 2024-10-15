using CommandLine;

namespace de.devcodemonkey.AIChecker.AIChecker.Commands
{
    [Verb("exportPromptRank", HelpText = "Export the ranking of the prompts")]
    public class ExportPromptRankVerb
    {
        [Option('r', "resultSet", Required = true, HelpText = "The result set name.")]
        public string ResultSet { get; set; }
    }
}
