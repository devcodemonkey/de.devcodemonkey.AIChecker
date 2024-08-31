using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace de.devcodemonkey.AIChecker.AIChecker.Commands
{
    [Verb("viewAverage", HelpText = "View the average time of the API request of a 'result set'.")]
    public class ViewAverageVerb
    {
        [Option('r', "ResultSet", Required = true, HelpText = "The result set name.")]
        //[Value(0, MetaName = "ResultSet", Required = true, HelpText = "The result set name.")]
        public string ResultSet { get; set; }
    }
}
