using Markdig;
using PuppeteerSharp;
using System.Text;

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
            if (!path.EndsWith(".md"))
                path += ".md";
            File.WriteAllText(path, Text?.ToString());
        }      

        private string ExportToHtml()
        {
            var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            return Markdown.ToHtml(Text.ToString(), pipeline);
        }

        public void ExportAsHtml(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Path is required.", nameof(path));
            if (!path.EndsWith(".html"))
                path += ".html";
            File.WriteAllText(path, ExportToHtml());
        }

        public async Task ExportToPdfAsync(string path)
        {
            string html = ExportToHtml();

            // Launch a headless Chromium browser without explicit download management
            using var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
            using var page = await browser.NewPageAsync();

            // Load the HTML and export it to a PDF file
            await page.SetContentAsync(html);
            if (!path.EndsWith(".pdf"))
                path += ".pdf";
            await page.PdfAsync(path);            
        }
    }
}
