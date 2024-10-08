﻿using de.devcodemonkey.AIChecker.CoreBusiness.DbModels;
using de.devcodemonkey.AIChecker.UseCases.Interfaces;
using de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;

namespace de.devcodemonkey.AIChecker.UseCases
{
    public class ViewResultSetsUseCase : IViewResultSetsUseCase
    {
        private readonly IDefaultMethodesRepository _defaultMethodesRepository;

        public ViewResultSetsUseCase(IDefaultMethodesRepository defaultMethodesRepository)
            => _defaultMethodesRepository = defaultMethodesRepository;

        public async Task<IEnumerable<Tuple<ResultSet, TimeSpan>>> ExecuteAsync()
        {
            var results = await _defaultMethodesRepository.GetAllEntitiesAsync<ResultSet>();
            List<Tuple<ResultSet, TimeSpan>> resultSets = new();
            foreach (var result in results)
            {
                var average = await _defaultMethodesRepository.ViewAverageTimeOfResultSet(result.ResultSetId);
                resultSets.Add(new Tuple<ResultSet, TimeSpan>(result, average));
            }
            return resultSets;
        }


    }
}
