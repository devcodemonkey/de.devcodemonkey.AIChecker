using de.devcodemonkey.AIChecker.CoreBusiness.DbModels;
using de.devcodemonkey.AIChecker.UseCases.Interfaces;
using de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace de.devcodemonkey.AIChecker.UseCases
{
    public class ViewResultsOfResultSetUseCase : IViewResultsOfResultSetUseCase
    {
        private readonly IDefaultMethodesRepository _defaultMethodesRepository;
        public ViewResultsOfResultSetUseCase(IDefaultMethodesRepository defaultMethodesRepository)
            => _defaultMethodesRepository = defaultMethodesRepository;

        public async Task<IEnumerable<Result>> ExecuteAsync(string resultSetId)
        {
            if (Guid.TryParse(resultSetId, out Guid guid))
                return await _defaultMethodesRepository.ViewResultsOfResultSetAsync(guid);
            else
                return await _defaultMethodesRepository.ViewResultsOfResultSetAsync(
                    await _defaultMethodesRepository.GetResultSetIdByValueAsync(resultSetId));
        }
    }
}
