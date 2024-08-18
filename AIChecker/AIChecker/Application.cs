using de.devcodemonkey.AIChecker.UseCases.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace de.devcodemonkey.AIChecker.AIChecker
{
    public class Application
    {
        private const int MAX_DISTANCE = 50;

        private readonly IImportQuestionAnswerUseCase _importQuestionAnswerUseCase;

        public Application(IImportQuestionAnswerUseCase importQuestionAnswerUseCase)
        {
            _importQuestionAnswerUseCase = importQuestionAnswerUseCase;
        }

        public async Task RunAsync(string[] args)
        {
            args = ["--importQuestionAnswer", "C:\\Users\\d-hoe\\source\\repos\\masterarbeit\\AIChecker\\Plugins\\JsonDeserializer\\Example.json"];



            if (args.Length == 2 && args[0].Equals("--importQuestionAnswer"))            
               await _importQuestionAnswerUseCase.ExecuteAsnc(args[1]);            
            else
            {
                Console.WriteLine("Commands:");
                MakeDistance("--importQuestionAnswer <path-to-file>", "Imports a Questions and Answers to the db");
            }
        }

        private void MakeDistance(string command, string description)
        {
            Console.WriteLine($"  {command}{new string(' ', MAX_DISTANCE - description.Length)} {description}");
        }
    }
}
