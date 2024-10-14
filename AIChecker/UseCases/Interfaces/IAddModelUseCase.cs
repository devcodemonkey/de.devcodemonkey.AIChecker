using de.devcodemonkey.AIChecker.CoreBusiness.DbModels;

namespace de.devcodemonkey.AIChecker.UseCases.Interfaces
{
    public interface IAddModelUseCase
    {
        Task ExecuteAsync(Model model);
    }
}