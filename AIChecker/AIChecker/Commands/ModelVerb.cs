﻿using CommandLine;

namespace de.devcodemonkey.AIChecker.AIChecker.Commands
{
    [Verb("model", HelpText = "Manage the models")]
    public class ModelVerb
    {
        [Option('v', "view", HelpText = "View all models")]
        public bool View { get; set; }

        [Option('a', "add", HelpText = "Add a new model")]
        public bool Add { get; set; }
    }
}
