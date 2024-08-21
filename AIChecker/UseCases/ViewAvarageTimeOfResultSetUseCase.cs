using de.devcodemonkey.AIChecker.UseCases.Interfaces;
using de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace de.devcodemonkey.AIChecker.UseCases
{
    public class ViewAvarageTimeOfResultSetUseCase : IViewAvarageTimeOfResultSetUseCase
    {
        private readonly IDefaultMethodesRepository _defaultMethodesRepository;

        public ViewAvarageTimeOfResultSetUseCase(IDefaultMethodesRepository defaultMethodesRepository)
            => _defaultMethodesRepository = defaultMethodesRepository;

        public async Task<TimeSpan> ExecuteAsync(string resultSet)
        {
            if (Guid.TryParse(resultSet, out Guid guid))
                return await _defaultMethodesRepository.ViewAvarageTimeOfResultSet(guid);
            return await _defaultMethodesRepository.ViewAvarageTimeOfResultSet(resultSet);
        }
    }
}
