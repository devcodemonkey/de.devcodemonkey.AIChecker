using CommandLine;

namespace de.devcodemonkey.AIChecker.AIChecker.Commands
{
    [Verb("checkjson", HelpText = "Check the JSON format of the results.")]
    public class CheckJsonVerb
    {
        [Option('r', "resultset", Required = false, HelpText = "The result set to check the JSON format of the results.")]
        public string? ResultSet { get; set; }
    }
}
