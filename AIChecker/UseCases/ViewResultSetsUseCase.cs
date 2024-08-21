using de.devcodemonkey.AIChecker.CoreBusiness.DbModels;
using de.devcodemonkey.AIChecker.UseCases.Interfaces;
using de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace de.devcodemonkey.AIChecker.UseCases
{
    public class ViewResultSetsUseCase : IViewResultSetsUseCase
    {
        private readonly IDefaultMethodesRepository _defaultMethodesRepository;

        public ViewResultSetsUseCase(IDefaultMethodesRepository defaultMethodesRepository)
            => _defaultMethodesRepository = defaultMethodesRepository;

        public async Task<IEnumerable<ResultSet>> ExecuteAsync()
            => await _defaultMethodesRepository.GetAllEntitiesAsync<ResultSet>();

    }
}
