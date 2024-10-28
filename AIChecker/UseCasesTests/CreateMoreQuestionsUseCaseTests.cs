using de.devcodemonkey.AIChecker.DataSource.APIRequester;
using de.devcodemonkey.AIChecker.DataStore.PostgreSqlEF;

namespace de.devcodemonkey.AIChecker.UseCases.Tests
{
    [TestClass()]
    public class CreateMoreQuestionsUseCaseTests
    {
        [TestMethod()]
        public async Task CreateMoreQuestionsUseCaseTest()
        {
            CreateMoreQuestionsUseCase createMoreQuestionsUseCase = new CreateMoreQuestionsUseCase(
                new APIRequester(), new DefaultMethodesRepository(new AicheckerContext()));


            // deactivating this test because it is a real request to the API
            // can be activated if needed


            //            await createMoreQuestionsUseCase.ExecuteAsync(
            //                "Test: More Questions",
            //                @"Du Antwortest nur im json-Format.  Das zu verwendende Format ist:
            //[
            //  {
            //    ""Question"": ""Beispielfrage 1"",    
            //  },
            //  {
            //    ""Question"" ""Beispielfrage 2"",    
            //  },
            //]
            //Du bist eine hilfesuchende Person, die Fragen  an einen IT-Support sendet.
            //Erstelle mir 10 Fragen auf Grundlage des folgenden Satzes:"
            //                );
        }
    }
}