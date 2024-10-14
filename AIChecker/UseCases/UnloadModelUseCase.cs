using de.devcodemonkey.AIChecker.UseCases.Interfaces;
using de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;

namespace de.devcodemonkey.AIChecker.UseCases
{
    public class UnloadModelUseCase : IUnloadModelUseCase
    {
        private readonly ILoadUnloadLms _loadUnloadLms;

        public UnloadModelUseCase(ILoadUnloadLms loadUnloadLms) => _loadUnloadLms = loadUnloadLms;

        public Task<bool> ExecuteAsync() => Task.FromResult(_loadUnloadLms.Unload());
    }
}
