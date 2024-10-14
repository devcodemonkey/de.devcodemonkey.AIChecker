using de.devcodemonkey.AIChecker.CoreBusiness.DbModels;

namespace de.devcodemonkey.AIChecker.UseCases
{
    public interface IViewModels
    {
        Task<IEnumerable<Model>> ExecuteAsync();
    }
}