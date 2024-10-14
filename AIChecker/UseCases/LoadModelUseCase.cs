using de.devcodemonkey.AIChecker.UseCases.Interfaces;
using de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;

namespace de.devcodemonkey.AIChecker.UseCases
{
    public class LoadModelUseCase : ILoadModelUseCase
    {
        private readonly ILoadUnloadLms _loadUnloadLms;

        public LoadModelUseCase(ILoadUnloadLms loadUnloadLms) => _loadUnloadLms = loadUnloadLms;

        public Task<bool> ExecuteAsync(string modelName) => Task.FromResult(_loadUnloadLms.Load(modelName));
    }
}
