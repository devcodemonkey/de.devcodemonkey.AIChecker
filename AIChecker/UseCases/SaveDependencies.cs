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
        public static async Task SaveDependenciesFromResult(
            IDefaultMethodesRepository defaultMethodesRepository,
            string systemPromt, 
            string resultSet, 
            IApiResult<ResponseData> apiResult, 
            Result result,
            string requestObject,
            string requestReason)
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

            // check if requestObject exists in db
            var requestObjectExists = await defaultMethodesRepository.ViewOverValue<RequestObject>(requestObject);
            if (requestObjectExists == null)
            {
                result.RequestObject = new RequestObject
                {
                    RequestObjectId = Guid.NewGuid(),
                    Value = requestObject
                };
            }
            else
                result.RequestObjectId = requestObjectExists.RequestObjectId;

            // check if requestReason exists in db
            var requestReasonExists = await defaultMethodesRepository.ViewOverValue<RequestReason>(requestReason);
            if (requestReasonExists == null)
            {
                result.RequestReason = new RequestReason
                {
                    RequestReasonId = Guid.NewGuid(),
                    Value = requestReason
                };
            }
            else
                result.RequestReasonId = requestReasonExists.RequestReasonId;
        }
    }
}
