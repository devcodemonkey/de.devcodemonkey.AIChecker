using CommandLine;

namespace de.devcodemonkey.AIChecker.AIChecker.Commands
{
    [Verb("model", HelpText = "Manage the models")]
    public class ModelVerb
    {
        [Option('v', "view", HelpText = "View all models")]
        public bool View { get; set; }

        [Option('a', "add", HelpText = "Add a new model")]
        public bool Add { get; set; }

        [Option('l', "load", HelpText = "Load a model")]
        public bool Load { get; set; }

        [Option('m', "modelname", HelpText = "(Optional) can use with load")]
        public string ModelName { get; set; }

        [Option('u', "unload", HelpText = "Unload a model")]
        public bool Unload { get; set; }
    }
}
