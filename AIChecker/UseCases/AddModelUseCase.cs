using de.devcodemonkey.AIChecker.CoreBusiness.DbModels;
using de.devcodemonkey.AIChecker.UseCases.Interfaces;
using de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;

namespace de.devcodemonkey.AIChecker.UseCases
{
    public class AddModelUseCase : IAddModelUseCase
    {
        private readonly IDefaultMethodesRepository _defaultMethodesRepository;

        public AddModelUseCase(IDefaultMethodesRepository defaultMethodesRepository) => _defaultMethodesRepository = defaultMethodesRepository;

        public async Task ExecuteAsync(Model model) => _defaultMethodesRepository.AddAsync(model);
    }
}
