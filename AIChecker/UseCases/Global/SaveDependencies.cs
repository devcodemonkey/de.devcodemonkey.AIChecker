﻿using de.devcodemonkey.AIChecker.CoreBusiness.DbModels;
using de.devcodemonkey.AIChecker.CoreBusiness.Models;
using de.devcodemonkey.AIChecker.DataSource.APIRequester.Interfaces;
using de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;

namespace de.devcodemonkey.AIChecker.UseCases.Global
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
            Model? modelExists = null;
            if (result.Model.Value != null)
                modelExists = await defaultMethodesRepository.ViewOverValue<Model>(result.Model.Value);
            if (modelExists == null)
            {
                result.Model = new Model
                {
                    ModelId = Guid.NewGuid(),
                    Value = apiResult?.Data?.Model ?? string.Empty
                };
            }
            else
            {
                result.ModelId = modelExists.ModelId;
                result.Model = null!;
            }

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
            var systemPromtExists = await defaultMethodesRepository.ViewOverValue<SystemPrompt>(systemPromt);
            if (systemPromtExists == null)
            {
                result.SystemPrompt = new SystemPrompt
                {
                    SystemPromptId = Guid.NewGuid(),
                    Value = systemPromt
                };
            }
            else
                result.SystemPromptId = systemPromtExists.SystemPromptId;

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
