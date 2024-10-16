
namespace de.devcodemonkey.AIChecker.UseCases.Interfaces
{
    public interface IExportPromptRatingUseCase
    {
        Task ExecuteAsync(DataExportType dataExportType, string resultSet);
    }
}