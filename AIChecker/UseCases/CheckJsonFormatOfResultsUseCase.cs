using de.devcodemonkey.AIChecker.CoreBusiness.DbModels;
using de.devcodemonkey.AIChecker.UseCases.Global;
using de.devcodemonkey.AIChecker.UseCases.Interfaces;
using de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;
using System.Collections.Generic;
using System.Text.Json;

namespace de.devcodemonkey.AIChecker.UseCases
{
    public class CheckJsonFormatOfResultsUseCase : ICheckJsonFormatOfResultsUseCase
    {
        private readonly IDefaultMethodesRepository _defaultMethodesRepository;

        public CheckJsonFormatOfResultsUseCase(IDefaultMethodesRepository defaultMethodesRepository)
            => _defaultMethodesRepository = defaultMethodesRepository;

        public async Task<IEnumerable<Result>> ExecuteAsync(string? ResultSet)
        {
            IEnumerable<Result> results;
            if (ResultSet == null)
                results = await _defaultMethodesRepository.GetAllEntitiesAsync<Result>();
            else
            {
                var resultSetId = await _defaultMethodesRepository.GetResultSetIdByValueAsync(ResultSet);
                results = await _defaultMethodesRepository.ViewResultsOfResultSetAsync(resultSetId);
            }

            foreach (var result in results)
                result.IsJson = JsonValidator.IsValidJson(result.Message);

            return results;
        }
    }
}
