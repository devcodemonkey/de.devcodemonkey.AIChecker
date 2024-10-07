using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownExporter
{
    public class MarkdownFile
    {
        public StringBuilder Text { get; }
        public MarkdownTable MarkdownTable { get; private set; }
        public MarkdownFontStyles MarkdownFontStyles { get; private set; }

        public MarkdownFile()
        {
            Text = new StringBuilder();
            MarkdownTable = new MarkdownTable(this);
            MarkdownFontStyles = new MarkdownFontStyles(this);
        }

        public void ExportAsMarkdown(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Path is required.", nameof(path));            
            File.WriteAllText(path, Text?.ToString());
        }        
    }
}
