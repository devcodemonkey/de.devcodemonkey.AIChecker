using CommandLine;
using de.devcodemonkey.AIChecker.UseCases;
using System.ComponentModel.DataAnnotations;

namespace de.devcodemonkey.AIChecker.AIChecker.Commands
{
    [Verb("exportPromptRank", HelpText = "Export the ranking of the prompts")]
    public class ExportPromptRankVerb
    {
        [Option('r', "resultSet", Required = true, HelpText = "The result set name.")]
        public string ResultSet { get; set; }
        
        [Option('t', "fileType", Required = false, HelpText = "The file type to export. Default is PDF. Possilbe is PDF, HTML and Markdown")]
        public string FileType { get; set; }

        [Option('o', "NotOpenFolder", Required = false, HelpText = "Don't open the folder for export")]
        public bool NotOpenFolder { get; set; }        
    }
}
