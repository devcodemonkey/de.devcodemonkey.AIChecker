using de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;
using System.Text;

namespace de.devcodemonkey.AIChecker.MarkdownExporter.Export
{
    public class ExportPromptRating : IExportPromptRating
    {
        public string GetTableTestdata(
            string datumDesAusdrucks,
            string testdatum,
            string resultSet,
            string anzahlDerAntworten,
            string geschaetzteToken,
            string temperatur)
        {
            var output = new StringBuilder();
            output.AppendLine(MdFontStyles.H3("Testdaten"));

            var table = new MdTable("Parameter", "Wert");

            // Adding rows to the table using the MdFontStyles for bold
            table.AddRow(
                MdFontStyles.Bold("Datum des Ausdrucks"),
                datumDesAusdrucks);

            table.AddRow(
                MdFontStyles.Bold("Testdatum"),
                testdatum);

            table.AddRow(
                MdFontStyles.Bold("ResultSet"),
                resultSet);

            table.AddRow(
                MdFontStyles.Bold("Anzahl der Antworten"),
                anzahlDerAntworten);

            table.AddRow(
                MdFontStyles.Bold("Geschätzte Token"),
                geschaetzteToken);

            table.AddRow(
                MdFontStyles.Bold("Temperatur"),
                temperatur);

            output.AppendLine(table.ToString());

            return output.ToString();
        }

        public string GetRunTable(
           int runNumber,
           string promptAnforderungen,
           string prompt,
           string systemPrompt,
           List<(string modelName, int rating)> modelRatings)
        {
            var output = new StringBuilder();
            output.AppendLine(MdFontStyles.H4($"{runNumber}. Durchlauf"));

            // Create the first table for Prompt Anforderungen, Prompt, and System Prompt
            var promptTable = new MdTable("Prompt Anfoderungen", "Prompt", "System Prompt");
            promptTable.AddRow(promptAnforderungen, prompt, systemPrompt);
            output.AppendLine(promptTable.ToString());

            // Create the second table for model evaluations
            var modelTable = new MdTable("Modell", "Ausgabe", "Bewertung (1-10)");

            int totalScore = 0;
            for (int i = 0; i < modelRatings.Count; i++)
            {
                var (modelName, rating) = modelRatings[i];
                totalScore += rating;
                modelTable.AddRow($"{i + 1}. {MdFontStyles.Bold(modelName)}", string.Empty, rating.ToString());
            }

            output.AppendLine(modelTable.ToString());

            // Append total score
            output.AppendLine(MdFontStyles.Bold($"Punkte Durchschnitt: {(double)totalScore / modelRatings.Count}"));

            return output.ToString();
        }

        public string GetModelDetailsTable(
            int modelNumber,
            string modelName,
            string baseModel,
            string modelDescriptionLink,
            string modelSize)
        {
            var output = new StringBuilder();
            output.AppendLine(MdFontStyles.Bold($"{modelNumber}. {modelName}"));
            output.AppendLine();

            // Create the table for model details
            var modelDetailsTable = new MdTable("Parameter", "Wert");

            // Adding rows to the table using the MdFontStyles for bold where necessary
            modelDetailsTable.AddRow(
                MdFontStyles.Bold("Modellname"),
                modelName);

            modelDetailsTable.AddRow(
                MdFontStyles.Bold("Basismodell/e"),
                baseModel);

            modelDetailsTable.AddRow(
                MdFontStyles.Bold("Link zur Beschreibung des Modells"),
                modelDescriptionLink);

            modelDetailsTable.AddRow(
                MdFontStyles.Bold("Größe des Modells (MB)"),
                modelSize);

            output.AppendLine(modelDetailsTable.ToString());

            return output.ToString();
        }
    }
}
