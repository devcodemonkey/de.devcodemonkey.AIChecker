using System.Text;

namespace de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;

public interface IMdFile
{
    StringBuilder Text { get; }

    void ExportAsHtml(string path);
    void ExportAsMarkdown(string path);
    Task ExportToPdfAsync(string path);
}
