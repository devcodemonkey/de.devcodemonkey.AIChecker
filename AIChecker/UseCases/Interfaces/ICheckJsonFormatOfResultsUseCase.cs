using de.devcodemonkey.AIChecker.CoreBusiness.DbModels;

namespace de.devcodemonkey.AIChecker.UseCases.Interfaces
{
    public interface ICheckJsonFormatOfResultsUseCase
    {
        Task<IEnumerable<Result>> ExecuteAsync(string? ResultSet);
    }
}