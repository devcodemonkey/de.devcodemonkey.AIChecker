
namespace de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;

public interface IExportPromptRating
{
    string GetRunTable(int runNumber, string promptAnforderungen, string prompt, string message, string systemPrompt, List<(string modelName, int rating)> modelRatings);
    string GetModelDetailsTable(int modelNumber, string modelName, string baseModel, string modelDescriptionLink, string modelSize);        
    string GetTableTestdata(string datumDesAusdrucks, string testdatum, string resultSet, string anzahlDerAntworten, string geschaetzteToken, string temperatur);    
}
