using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace de.devcodemonkey.AIChecker.AIChecker.Commands
{
    [Verb("importQuestions", HelpText = "Imports Questions and Answers to the db.")]
    public class ImportQuestionsVerb
    {
        [Option('p', "path", Required = true, HelpText = "Path to the file with Questions and Answers.")]
        public string Path { get; set; }
    }
}
