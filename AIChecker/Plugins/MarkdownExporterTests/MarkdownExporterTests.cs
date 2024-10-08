using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Threading.Tasks;

namespace MarkdownExporter.Tests
{
    [TestClass]
    public class MarkdownExporterTests
    {
        private const string OutputDirectory = "TestOutput";
        private const string MarkdownFileName = "test_markdown";
        private string MarkdownFilePath => Path.Combine(OutputDirectory, $"{MarkdownFileName}.md");
        private string HtmlFilePath => Path.Combine(OutputDirectory, $"{MarkdownFileName}.html");
        private string PdfFilePath => Path.Combine(OutputDirectory, $"{MarkdownFileName}.pdf");

        [TestInitialize]
        public void Setup()
        {
            // Ensure output directory is clean before each test
            if (Directory.Exists(OutputDirectory))
            {
                Directory.Delete(OutputDirectory, true);
            }
            Directory.CreateDirectory(OutputDirectory);
        }

        [TestMethod]
        public async Task TestMarkdownExport()
        {
            // Arrange: Create the markdown file with data
            var markdownFile = new MdFile();

            // Add test heading and tables as described in the input
            var fontStyles = markdownFile.MarkdownFontStyles;
            fontStyles.AddH2Text("Test");
            fontStyles.AddH3Text("Testdaten:");

            // Add a table for test data
            var testDataTable = new MdTable(markdownFile, MdFontStyles.Bold("Parameter"), MdFontStyles.Bold("Wert"));
            testDataTable.AddRow(MdFontStyles.Bold("Datum des Ausdrucks"), "[Datum des Ausdrucks hier einfügen]");
            testDataTable.AddRow(MdFontStyles.Bold("Testdatum"), "[Testdatum hier einfügen]");
            testDataTable.AddRow(MdFontStyles.Bold("Zieldefinition"), "");
            testDataTable.AddRow(MdFontStyles.Bold("Fragestellung"), "");
            testDataTable.AddRow(MdFontStyles.Bold("Anzahl der Antworten"), "[Anzahl der Antworten hier einfügen]");
            testDataTable.AddTable();

            fontStyles.AddH3Text("Bewertungsbogen:");
            fontStyles.AddH4Text("Modellkonfigurationen:");

            // Add a table for model configurations
            var modelConfigTable = new MdTable(markdownFile, MdFontStyles.Bold("Modell"), MdFontStyles.Bold("Temperatur"), MdFontStyles.Bold("Geschätzte Token"));
            modelConfigTable.AddRow("1. {Modellname}", "", "");
            modelConfigTable.AddRow("2. {Modellname}", "", "");
            modelConfigTable.AddRow("3. {Modellname}", "", "");
            modelConfigTable.AddRow("4. {Modellname}", "", "");
            modelConfigTable.AddTable();

            fontStyles.AddH4Text("Auswertung");

            // Add a table for evaluation
            var evaluationTable = new MdTable(markdownFile, MdFontStyles.Bold("Modell"), MdFontStyles.Bold("Bewertung (1-10)"), MdFontStyles.Bold("Punkte"), MdFontStyles.Bold("Frage"), MdFontStyles.Bold("Ausgabe"));
            evaluationTable.AddRow("1. {Modellname}", "5", "", "", "");
            evaluationTable.AddRow("2. {Modellname}", "3", "", "", "");
            evaluationTable.AddRow("3. {Modellname}", "8", "", "", "");
            evaluationTable.AddRow("4. {Modellname}", "6", "", "", "");
            evaluationTable.AddTable();

            // Act: Export to markdown, HTML, and PDF
            markdownFile.ExportAsMarkdown(MarkdownFilePath);
            markdownFile.ExportAsHtml(HtmlFilePath);
            await markdownFile.ExportToPdfAsync(PdfFilePath);

            // Assert: Check if the files are created
            Assert.IsTrue(File.Exists(MarkdownFilePath), "Markdown file not found.");
            Assert.IsTrue(File.Exists(HtmlFilePath), "HTML file not found.");
            Assert.IsTrue(File.Exists(PdfFilePath), "PDF file not found.");
        }
    }
}
