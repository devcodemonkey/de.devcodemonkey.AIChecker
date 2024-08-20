using de.devcodemonkey.AIChecker.CoreBusiness.DbModels;
using de.devcodemonkey.AIChecker.CoreBusiness.Models;
using de.devcodemonkey.AIChecker.DataSource.APIRequester.Interfaces;
using de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace de.devcodemonkey.AIChecker.UseCases
{
    public static class SaveDependencies
    {
        public static async Task SaveDependenciesFromResult(IDefaultMethodesRepository defaultMethodesRepository,string systemPromt, string resultSet, IApiResult<ResponseData> apiResult, Result result)
        {
            // check if model exists in db
            var modelExists = await defaultMethodesRepository.ViewOverValue<Model>(apiResult.Data.Model);
            if (modelExists == null)
            {
                result.Model = new Model
                {
                    ModelId = Guid.NewGuid(),
                    Value = apiResult.Data.Model
                };
            }
            else
                result.ModelId = modelExists.ModelId;

            // check if resultSet exists in db
            var resultSetExists = await defaultMethodesRepository.ViewOverValue<ResultSet>(resultSet);
            if (resultSetExists == null)
            {
                result.ResultSet = new ResultSet
                {
                    ResultSetId = Guid.NewGuid(),
                    Value = resultSet
                };
            }
            else
                result.ResultSetId = resultSetExists.ResultSetId;

            // check if systemPromt exists in db
            var systemPromtExists = await defaultMethodesRepository.ViewOverValue<SystemPromt>(systemPromt);
            if (systemPromtExists == null)
            {
                result.SystemPromt = new SystemPromt
                {
                    SystemPromtId = Guid.NewGuid(),
                    Value = systemPromt
                };
            }
            else
                result.SystemPromtId = systemPromtExists.SystemPromtId;
        }
    }
}
