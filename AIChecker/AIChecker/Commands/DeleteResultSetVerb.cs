using CommandLine;

namespace de.devcodemonkey.AIChecker.AIChecker.Commands
{
    [Verb("deleteResultSet", HelpText = "Deletes a 'result set'.")]
    public class DeleteResultSetVerb
    {
        [Option('r', "ResultSet", Required = true, HelpText = "The result set name.")]
        public string? ResultSet { get; set; }
    }
}
