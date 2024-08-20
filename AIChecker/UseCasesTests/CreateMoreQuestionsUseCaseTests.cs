using Microsoft.VisualStudio.TestTools.UnitTesting;
using de.devcodemonkey.AIChecker.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using de.devcodemonkey.AIChecker.DataSource.APIRequester;
using de.devcodemonkey.AIChecker.DataStore.SQLServerEF;

namespace de.devcodemonkey.AIChecker.UseCases.Tests
{
    [TestClass()]
    public class CreateMoreQuestionsUseCaseTests
    {
        [TestMethod()]
        public async Task CreateMoreQuestionsUseCaseTest()
        {
            CreateMoreQuestionsUseCase createMoreQuestionsUseCase = new CreateMoreQuestionsUseCase(
                new APIRequester(), new DefaultMethodesRepository());

            await createMoreQuestionsUseCase.ExecuteAsync(
                "Test: More Questions",
                @"Du Antwortest nur im json-Format.  Das zu verwendende Format ist:
[
  {
    ""Question"": ""Beispielfrage 1"",    
  },
  {
    ""Question"" ""Beispielfrage 2"",    
  },
]
Du bist eine hilfesuchende Person, die Fragen  an einen IT-Support sendet.
Erstelle mir 10 Fragen auf Grundlage des folgenden Satzes:"
                );
        }
    }
}