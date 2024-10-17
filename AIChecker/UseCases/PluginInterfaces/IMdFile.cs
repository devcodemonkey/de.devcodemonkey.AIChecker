using de.devcodemonkey.AIChecker.CoreBusiness.MarkDownExporterModels;
using System.Text;

namespace de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;

public interface IMdFile
{
    StringBuilder Text { get; }

    Task Export(string path, DataExportType dataExportType);
    void ExportAsHtml(string path);
    void ExportAsMarkdown(string path);
    Task ExportToPdfAsync(string path);
}
