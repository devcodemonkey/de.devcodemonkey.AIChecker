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
    public class DeleteAllQuestionAnswerUseCase : IDeleteAllQuestionAnswerUseCase
    {
        private readonly IDefaultMethodesRepository _defaultMethodesRepository;

        public DeleteAllQuestionAnswerUseCase(IDefaultMethodesRepository defaultMethodesRepository)
        {
            _defaultMethodesRepository = defaultMethodesRepository;
        }

        public async Task ExecuteAsync()
        {
            await _defaultMethodesRepository.RemoveAllEntitiesAsync<Answer>();
            await _defaultMethodesRepository.RemoveAllEntitiesAsync<Question>();
        }
    }
}
