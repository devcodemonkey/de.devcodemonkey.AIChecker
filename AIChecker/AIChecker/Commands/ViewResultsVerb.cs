using CommandLine;

namespace de.devcodemonkey.AIChecker.AIChecker.Commands
{
    [Verb("viewResults", HelpText = "View Results of a result set")]
    public class ViewResultsVerb
    {
        [Option('r', "ResultSet", Required = true, HelpText = "The result set name.")]
        public string? ResultSet { get; set; }

        [Option('f', "FormatTable", Required = false, Default = false, HelpText = "Format the table for a better view.")]
        public bool FortmatTable { get; set; }
    }
}
