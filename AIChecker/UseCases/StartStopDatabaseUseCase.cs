using de.devcodemonkey.AIChecker.DataStore.PostgreSqlEF;
using de.devcodemonkey.AIChecker.UseCases.Interfaces;

namespace de.devcodemonkey.AIChecker.UseCases
{
    public class StartStopDatabaseUseCase : IStartStopDatabaseUseCase
    {
        private readonly IWslDatabaseService _wslDatabaseService;

        public StartStopDatabaseUseCase(IWslDatabaseService wslDatabaseService) => _wslDatabaseService = wslDatabaseService;

        public Task<bool> ExecuteAsync(bool start)
        {
            if (start)
                return Task.FromResult(_wslDatabaseService.StartDatabase());
            else
                return Task.FromResult(_wslDatabaseService.StopDatabase());
        }
    }
}
