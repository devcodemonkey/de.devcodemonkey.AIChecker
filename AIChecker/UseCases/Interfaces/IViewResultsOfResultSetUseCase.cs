using de.devcodemonkey.AIChecker.CoreBusiness.DbModels;

namespace de.devcodemonkey.AIChecker.UseCases.Interfaces
{
    public interface IViewResultsOfResultSetUseCase
    {        
        Task<IEnumerable<Result>> ExecuteAsync(string resultSetId);
    }
}