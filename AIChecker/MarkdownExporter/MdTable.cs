using de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;
using System.Text.RegularExpressions;

namespace de.devcodemonkey.AIChecker.MarkdownExporter
{
    public class MdTable : IMdTable
    {
        private MdFile? File { get; }
        private List<string> Headers { get; }
        private List<List<string>> Rows { get; }

        public MdTable(params string[] headers)
        {
            Headers = new List<string>(headers);
            Rows = new List<List<string>>();
        }

        public MdTable(MdFile file, params string[] headers) : this(headers)
        {
            File = file;
        }

        public void AddRow(params string[] values)
        {
            for (int i = 0; i < values.Length; i++)
                //values[i] = values[i]?.Replace("\r\n", "<br>")  // Windows-style newlines
                //                 .Replace("\n", "<br>") ?? values[i];   // Unix-style newlines
                if (values[i] != null)
                {
                    values[i] = Regex.Replace(values[i], @"^ +", match =>
                    {
                        // Replace each leading space with &nbsp;
                        return string.Concat(match.Value.Select(_ => "&nbsp;"));
                    }, RegexOptions.Multiline)  // Apply replacement at the start of each line
                    .Replace("\r\n", "<br>")  // Windows-style newlines
                    .Replace("\n", "<br>");   // Unix-style newlines
                }
            Rows.Add(new List<string>(values));
        }

        public override string ToString()
        {
            string table = "| " + string.Join(" | ", Headers) + " |\n";
            table += "| " + string.Join(" | ", new string[Headers.Count].Select(_ => "---")) + " |\n";

            foreach (var row in Rows)
            {
                table += "| " + string.Join(" | ", row) + " |\n";
            }

            return table;
        }

        public void AddTable()
        {
            if (File == null)
                throw new InvalidOperationException("File is not set.");
            File.Text.AppendLine(ToString());
        }
    }
}
