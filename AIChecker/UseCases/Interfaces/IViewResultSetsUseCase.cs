﻿using de.devcodemonkey.AIChecker.CoreBusiness.DbModels;

namespace de.devcodemonkey.AIChecker.UseCases.Interfaces
{
    public interface IViewResultSetsUseCase
    {
        Task<IEnumerable<ResultSet>> ExecuteAsync();
    }
}