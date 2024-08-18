using Microsoft.VisualStudio.TestTools.UnitTesting;
using de.devcodemonkey.AIChecker.DataStore.SQLServerEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using de.devcodemonkey.AIChecker.CoreBusiness.DbModels;

namespace de.devcodemonkey.AIChecker.DataStore.SQLServerEF.Tests
{
    [TestClass()]
    public class DefaultMethodesRepositoryTests
    {
        [TestMethod()]
        public async Task AddAsyncTest()
        {
            DefaultMethodesRepository defaultMethodesRepository = new DefaultMethodesRepository();

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
    }
}