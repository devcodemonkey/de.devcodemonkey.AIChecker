using CommandLine;

namespace de.devcodemonkey.AIChecker.AIChecker.Commands
{
    [Verb("model", HelpText = "Manage the models")]
    public class ModelVerb
    {
        [Option('a', "add", HelpText = "Add a new model")]
        public bool Add { get; set; }
    }
}
