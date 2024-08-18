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
        private const int MAX_DISTANCE = 40;
        private const int MONKEY_DISTANCE = 12;

        private readonly IImportQuestionAnswerUseCase _importQuestionAnswerUseCase;
        private readonly IDeleteAllQuestionAnswerUseCase _deleteAllQuestionAnswerUseCase;

        public Application(IImportQuestionAnswerUseCase importQuestionAnswerUseCase,
            IDeleteAllQuestionAnswerUseCase deleteAllQuestionAnswerUseCase)
        {
            _importQuestionAnswerUseCase = importQuestionAnswerUseCase;
            _deleteAllQuestionAnswerUseCase = deleteAllQuestionAnswerUseCase;
        }

        public async Task RunAsync(string[] args)
        {
            //args = ["--importQuestionAnswer", "C:\\Users\\d-hoe\\source\\repos\\masterarbeit\\AIChecker\\Plugins\\JsonDeserializer\\Example.json"];
            //args = ["--deleteAllEntityQuestionAnswer"];
            if (args.Length == 2 && args[0].Equals("--importQuestionAnswer"))
                await _importQuestionAnswerUseCase.ExecuteAsnc(args[1]);
            else if (args.Length == 1 && args[0].Equals("--deleteAllEntityQuestionAnswer"))
                await _deleteAllQuestionAnswerUseCase.ExecuteAsync();
            else
            {
                Console.WriteLine("Commands:");
                MakeDistance("--importQuestionAnswer <path-to-file>", "Imports a Questions and Answers to the db");
                MakeDistance("--deleteAllEntityQuestionAnswer", "Deletes all Questions and Answers from the db");
            }
            CreateMonkey();
        }

        private void MakeDistance(string command, string description)
        {
            Console.WriteLine($"  {command}{new string(' ', MAX_DISTANCE - command.Length)} {description}");
        }

        private void CreateMonkey()
        {
            string monkey = @"
            __,__
  .--.  .-""     ""-.  .--.
 / .. \/  .-. .-.  \/ .. \
| |  '|  /   Y   \  |'  | |
| \   \  \ 0 | 0 /  /   / |
 \ '- ,\.-""""""""""""""-./, -' /
  `'-' /_   ^ ^   _\ '-'`
      |  \._   _./  |
      \   \ `~` /   /
       '._ '-=-' _.'
          '~---~'
   |------------------|
   | devcodemonkey.de |
   |------------------|";

            foreach (var line in monkey.Split('\n'))
            {
                Console.WriteLine($"{new string(' ', MAX_DISTANCE - MONKEY_DISTANCE)} {line}");
            }
        }
    }
}
