using de.devcodemonkey.AIChecker.CoreBusiness.MarkDownExporterModels;
using de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using HtmlToOpenXml;
using Markdig;
using PuppeteerSharp;
using System.Net.Http;
using System.Text;

namespace de.devcodemonkey.AIChecker.MarkdownExporter
{
    public class MdFile : IMdFile
    {
        public StringBuilder Text { get; }        

        public MdFile()
        {
            Text = new StringBuilder();            
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
                //.UseBootstrap()
                .Build();

            // add footer
            Text.AppendLine($@"<footer class=""text-dark text-center py-3"">
  <div class=""container"">
    <p class=""mb-0"">&copy; {DateTime.Today.Year} AiChecker.  Licensed under the <a href=""https://github.com/devcodemonkey/de.devcodemonkey.AIChecker?tab=MIT-1-ov-file"" target=""_blank"">MIT License</a></p>
  </div>
</footer>");

            var html = Markdown.ToHtml(Text.ToString(), pipeline);

            // Add Bootstrap's table classes for styling
            html = html.Replace("<table>", "<table class=\"table table-striped table-bordered\">");

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

        public void ExportAsDocx(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Path is required.", nameof(path));
            if (!path.EndsWith(".docx"))
                path += ".docx";

            var html = ExportToHtml();
            
            // Convert HTML to DOCX
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Create(path, DocumentFormat.OpenXml.WordprocessingDocumentType.Document))
            {
                // Add a main document part
                MainDocumentPart mainPart = wordDoc.AddMainDocumentPart();
                mainPart.Document = new Document();
                Body body = new Body();
                mainPart.Document.Append(body);

                // Convert HTML to OpenXml
                HtmlConverter converter = new HtmlConverter(mainPart);
                var paragraphs = converter.Parse(html);

                // Append paragraphs to the DOCX document
                foreach (var paragraph in paragraphs)
                {
                    body.Append(paragraph);
                }

                // Save changes to the DOCX file
                mainPart.Document.Save();
            }
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

        public async Task Export(string path, DataExportType dataExportType)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Path is required.", nameof(path));

            switch (dataExportType)
            {
                case DataExportType.Markdown:
                    ExportAsMarkdown(path);
                    break;
                case DataExportType.Html:
                    ExportAsHtml(path);
                    break;
                case DataExportType.Docx:
                    ExportAsDocx(path);
                    break;
                default:
                    await ExportToPdfAsync(path);
                    break;
            }
        }
    }
}
