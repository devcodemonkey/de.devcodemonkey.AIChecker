
namespace de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;

public interface IExportPromptRating
{
    string GetTableTestdata(string datumDesAusdrucks, string testdatum, string resultSet, string? description, string anzahlDerAntworten, string geschaetzteToken, string temperatur);
    string GetRunTable(int runNumber, string promptAnforderungen, string prompt, string systemPrompt, List<(string message, string modelName, int rating, string reason)> modelRatings);
    string GetModelDetailsTable(int modelNumber, string modelName, string baseModel, string modelDescriptionLink, string modelSize, string parameter);
}
