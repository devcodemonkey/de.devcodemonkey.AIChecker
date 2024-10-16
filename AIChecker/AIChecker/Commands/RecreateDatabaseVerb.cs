using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace de.devcodemonkey.AIChecker.AIChecker.Commands
{
    [Verb("recreateDatabase", HelpText = "Recreates the database.")]
    public class RecreateDatabaseVerb
    {
        [Option('f', "force", Required = false, HelpText = "Force the recreation of the database.")]
        public bool Force { get; set; }
    }
}
