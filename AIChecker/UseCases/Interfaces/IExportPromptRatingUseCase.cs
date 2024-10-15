
namespace de.devcodemonkey.AIChecker.UseCases
{
    public interface IExportPromptRatingUseCase
    {
        Task ExecuteAsync(DataExportType dataExportType, string resultSet);
    }
}