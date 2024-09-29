using de.devcodemonkey.AIChecker.UseCases.Interfaces;
using de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;

namespace de.devcodemonkey.AIChecker.UseCases
{
    public class ViewAverageTimeOfResultSetUseCase : IViewAverageTimeOfResultSetUseCase
    {
        private readonly IDefaultMethodesRepository _defaultMethodesRepository;

        public ViewAverageTimeOfResultSetUseCase(IDefaultMethodesRepository defaultMethodesRepository)
            => _defaultMethodesRepository = defaultMethodesRepository;

        public async Task<TimeSpan> ExecuteAsync(string resultSet)
        {
            if (Guid.TryParse(resultSet, out Guid guid))
                return await _defaultMethodesRepository.ViewAverageTimeOfResultSet(guid);
            return await _defaultMethodesRepository.ViewAverageTimeOfResultSet(
                await _defaultMethodesRepository.GetResultSetIdByValueAsync(resultSet));
        }
    }
}
