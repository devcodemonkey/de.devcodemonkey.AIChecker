using de.devcodemonkey.AIChecker.CoreBusiness.DbModels;
using de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;

namespace de.devcodemonkey.AIChecker.UseCases
{
    public class ViewModels : IViewModels
    {
        private readonly IDefaultMethodesRepository _defaultMethodesRepository;

        public ViewModels(IDefaultMethodesRepository defaultMethodesRepository) => _defaultMethodesRepository = defaultMethodesRepository;

        public async Task<IEnumerable<Model>> ExecuteAsync() => await _defaultMethodesRepository.GetAllEntitiesAsync<Model>();
    }
}
