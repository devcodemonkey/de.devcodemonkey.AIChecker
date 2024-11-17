using CommandLine;

namespace de.devcodemonkey.AIChecker.AIChecker.Commands
{
    [Verb("checkJson", HelpText = "Check the JSON format of the results.")]
    public class CheckJsonVerb
    {
        [Option('r', "resultset", Required = false, HelpText = "The result set to check the JSON format of the results.")]
        public string? ResultSet { get; set; }

        [Option('o', "showOutput", Default = false, Required = false, HelpText = "Show the output of the results.")]
        public bool ShowOutput { get; set; }
    }
}
