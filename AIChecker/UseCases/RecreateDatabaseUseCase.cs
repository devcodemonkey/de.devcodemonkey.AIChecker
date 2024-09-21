using de.devcodemonkey.AIChecker.UseCases.Interfaces;
using de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;

namespace de.devcodemonkey.AIChecker.UseCases
{
    public class RecreateDatabaseUseCase : IRecreateDatabaseUseCase
    {
        private readonly IDefaultMethodesRepository _defaultMethodesRepository;

        public RecreateDatabaseUseCase(IDefaultMethodesRepository defaultMethodesRepository)
            => _defaultMethodesRepository = defaultMethodesRepository;

        public async Task ExecuteAysnc()
            => await _defaultMethodesRepository.RecreateDatabaseAsync();
    }
}
