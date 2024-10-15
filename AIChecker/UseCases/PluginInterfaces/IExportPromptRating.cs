
namespace de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;

public interface IExportPromptRating
{
    string GetFirstRunTable(int runNumber, string promptAnforderungen, string prompt, string systemPrompt, List<(string modelName, int rating)> modelRatings);
    string GetModelDetailsTable(int modelNumber, string modelName, string baseModel, string modelDescriptionLink, string modelSize);
    string GetTableTestdata(string datumDesAusdrucks, string testdatum, string anzahlDerAntworten, string zieldefinition = "", string fragestellung = "", string geschaetzteToken = "", string temperatur = "");
}
