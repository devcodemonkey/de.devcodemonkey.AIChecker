using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace de.devcodemonkey.AIChecker.AIChecker
{
    // Define options class for CommandLineParser
    public class Options
    {
        [Option("importQuestionAnswer", HelpText = "Imports a Questions and Answers to the db.")]
        public string ImportQuestionAnswer { get; set; }

        [Option("viewAverage", HelpText = "Views the average time of the API request of a 'result set'.")]
        public string ViewAverage { get; set; }

        [Option("viewResultSets", HelpText = "Views all result sets.")]
        public bool ViewResultSets { get; set; }

        [Option("deleteAllEntityQuestionAnswer", HelpText = "Deletes all Questions and Answers from the db.")]
        public bool DeleteAllEntityQuestionAnswer { get; set; }

        [Option("createMoreQuestions", HelpText = "Create more questions under the 'system prompt' and save them under the result 'set name'.")]
        public string CreateMoreQuestions { get; set; }

        [Option("systemPrompt", HelpText = "The system prompt or path (path:<file-path>) for creating questions.")]
        public string SystemPrompt { get; set; }
    }
}
