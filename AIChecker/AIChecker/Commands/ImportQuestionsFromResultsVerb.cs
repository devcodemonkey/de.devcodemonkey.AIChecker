using CommandLine;

namespace de.devcodemonkey.AIChecker.AIChecker.Commands
{
    [Verb("importQuestionsFromResults", HelpText = "Imports Questions from Results to the db.")]
    public class ImportQuestionsFromResultsVerb
    {
        [Option('r', "resultSet", Required = true, HelpText = "Name of the ResultSet.")]
        public string ResultSet { get; set; }

        [Option('c', "category", Required = true, HelpText = "Category of the Questions.")]
        public string Category { get; set; }
    }
}
