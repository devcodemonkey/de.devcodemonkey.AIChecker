

namespace de.devcodemonkey.AIChecker.UseCases.Interfaces
{
    public interface IExportPromptRatingUseCase
    {   
        Task ExecuteAsync(string resultSet, DataExportType dataExportType = DataExportType.Pdf, bool openFolder = true);
    }
}