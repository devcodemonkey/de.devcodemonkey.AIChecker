﻿using de.devcodemonkey.AIChecker.UseCases.Interfaces;
using de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;

namespace de.devcodemonkey.AIChecker.UseCases
{
    public class DeleteResultSetUseCase : IDeleteResultSetUseCase
    {
        private readonly IDefaultMethodesRepository _defaultMethodesRepository;

        public DeleteResultSetUseCase(IDefaultMethodesRepository defaultMethodesRepository)
            => _defaultMethodesRepository = defaultMethodesRepository;

        public async Task ExecuteAsync(string resultSet)
        {
            if (Guid.TryParse(resultSet, out Guid guid))
                await _defaultMethodesRepository.RemoveResultSetAsync(guid);
            else
                await _defaultMethodesRepository.RemoveResultSetAsync(
                    await _defaultMethodesRepository.GetResultSetIdByValueAsync(resultSet));
        }
    }
}
