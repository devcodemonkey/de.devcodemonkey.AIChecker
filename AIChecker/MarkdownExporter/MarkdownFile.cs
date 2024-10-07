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
            var pipeline = new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .UseBootstrap()
                .Build();

            // add footer
            Text.AppendLine($@"<footer class=""text-dark text-center py-3"">
  <div class=""container"">
    <p class=""mb-0"">&copy; {DateTime.Today.Year} AiChecker.  Licensed under the <a href=""https://github.com/devcodemonkey/de.devcodemonkey.AIChecker?tab=MIT-1-ov-file"" target=""_blank"">MIT License</a></p>
  </div>
</footer>");

            var html = Markdown.ToHtml(Text.ToString(), pipeline);

            // Inject padding directly to the body element
            var css = @"<style>body { padding: 50px; }</style>
<link href=""https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css"" rel=""stylesheet"" integrity=""sha384-QWTKZyjpPEjISv5WaRU9OFeRpok6YctnYmDr5pNlyT2bRjXh0JMhjY6hW+ALEwIH"" crossorigin=""anonymous"">";

            // Wrap the content with <html>, <head> and <body> tags and apply the style
            return $"<html><head>{css}</head><body>{html}</body></html>";
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
            await new BrowserFetcher().DownloadAsync();
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
