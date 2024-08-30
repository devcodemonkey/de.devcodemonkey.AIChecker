using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace de.devcodemonkey.AIChecker.AIChecker.Commands
{
    [Verb("viewResults", HelpText = "View Results of a result set")]
    public class ViewResultsVerb
    {
        [Option('r', "ResultSet", Required = true, HelpText = "The result set name.")]
        public string ResultSet { get; set; }
    }
}
