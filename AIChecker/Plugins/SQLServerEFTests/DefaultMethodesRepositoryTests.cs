using Microsoft.VisualStudio.TestTools.UnitTesting;
using de.devcodemonkey.AIChecker.DataStore.SQLServerEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using de.devcodemonkey.AIChecker.CoreBusiness.DbModels;
using de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;

namespace de.devcodemonkey.AIChecker.DataStore.SQLServerEF.Tests
{
    [TestClass()]
    public class DefaultMethodesRepositoryTests
    {
        [TestMethod()]
        public async Task AddAsyncTest()
        {
            IDefaultMethodesRepository defaultMethodesRepository = new DefaultMethodesRepository();

            Question question = new Question
            {
                QuestionId = Guid.NewGuid(),
                Value = "Test",
                Answer = new Answer
                {
                    AnswerId = Guid.NewGuid(),
                    Value = "Test"
                }
            };

            await defaultMethodesRepository.AddAsync(question);


        }

        [TestMethod()]
        public async Task ViewAvarageTimeOfResultSetTest()
        {
            IDefaultMethodesRepository defaultMethodesRepository = new DefaultMethodesRepository();

            var avgTime = await defaultMethodesRepository.ViewAvarageTimeOfResultSet(
                await defaultMethodesRepository.GetResultSetIdByValueAsync("syntetic questions creation | over 25 questions | area: outlook | model: em german mistral v01 7B"));

            var seconds = avgTime.TotalSeconds;

        }
    }
}