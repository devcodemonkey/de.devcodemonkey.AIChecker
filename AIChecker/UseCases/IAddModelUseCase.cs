using de.devcodemonkey.AIChecker.CoreBusiness.DbModels;

namespace de.devcodemonkey.AIChecker.UseCases
{
    public interface IAddModelUseCase
    {        
        Task ExecuteAsync(Model model);
    }
}