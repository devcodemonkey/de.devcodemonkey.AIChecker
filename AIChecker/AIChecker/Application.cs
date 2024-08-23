using de.devcodemonkey.AIChecker.UseCases.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using Microsoft.Extensions.Options;


namespace de.devcodemonkey.AIChecker.AIChecker
{
    public class Application
    {
        private const int MAX_DISTANCE = 58;
        private const int MONKEY_DISTANCE = 12;

        private readonly IImportQuestionAnswerUseCase _importQuestionAnswerUseCase;
        private readonly IDeleteAllQuestionAnswerUseCase _deleteAllQuestionAnswerUseCase;
        private readonly ICreateMoreQuestionsUseCase _createMoreQuestionsUseCase;
        private readonly IViewAvarageTimeOfResultSetUseCase _viewAvarageTimeOfResultSetUseCase;
        private readonly IViewResultSetsUseCase _viewResultSetsUseCase;

        public Application(IImportQuestionAnswerUseCase importQuestionAnswerUseCase,
            IDeleteAllQuestionAnswerUseCase deleteAllQuestionAnswerUseCase,
            ICreateMoreQuestionsUseCase createMoreQuestionsUseCase,
            IViewAvarageTimeOfResultSetUseCase viewAvarageTimeOfResultSetUseCase,
            IViewResultSetsUseCase viewResultSetsUseCase)
        {
            _importQuestionAnswerUseCase = importQuestionAnswerUseCase;
            _deleteAllQuestionAnswerUseCase = deleteAllQuestionAnswerUseCase;
            _createMoreQuestionsUseCase = createMoreQuestionsUseCase;
            _viewAvarageTimeOfResultSetUseCase = viewAvarageTimeOfResultSetUseCase;
            _viewResultSetsUseCase = viewResultSetsUseCase;
        }

        public async Task RunAsync(string[] args)
        {
            if (args.Length == 0)
            {
                await ViewResultSets();
                CreateMonkey();
                return;
            }
            //args = ["--importQuestionAnswer", "C:\\Users\\d-hoe\\source\\repos\\masterarbeit\\AIChecker\\Plugins\\JsonDeserializer\\Example.json"];
            //args = ["--viewAverage", "Test set"];
            //args = ["--deleteAllEntityQuestionAnswer"];
            //args = ["--importQuestionAnswer", "C:\\Users\\d-hoe\\source\\repos\\masterarbeit.wiki\\06_00_00-Ticketexport\\FAQs\\FAQ-Outlook.json"];
            //args = ["--createMoreQuestions", "Test set",
            //    "path:C:\\Users\\d-hoe\\source\\repos\\masterarbeit\\AIChecker\\AIChecker\\examples\\system_promt.txt"];
            var parsingTask = Parser.Default.ParseArguments<Options>(args)
                .WithParsedAsync(async (Options opts) =>
                {
                    if (opts.ImportQuestionAnswer != null)
                    {
                        await _importQuestionAnswerUseCase.ExecuteAsnc(opts.ImportQuestionAnswer);
                    }
                    else if (opts.ViewAverage != null)
                    {
                        var result = await _viewAvarageTimeOfResultSetUseCase.ExecuteAsync(opts.ViewAverage);
                        Console.WriteLine($"{result.TotalSeconds} seconds");
                    }
                    else if (opts.ViewResultSets)
                    {
                        await ViewResultSets();
                    }
                    else if (opts.DeleteAllEntityQuestionAnswer)
                    {
                        await _deleteAllQuestionAnswerUseCase.ExecuteAsync();
                    }
                    else if (opts.CreateMoreQuestions != null)
                    {
                        if (opts.SystemPrompt != null && opts.SystemPrompt.StartsWith("path:"))
                        {
                            var filePath = opts.SystemPrompt.Substring(5);
                            var promptContent = File.ReadAllText(filePath);
                            await _createMoreQuestionsUseCase.ExecuteAsync(opts.CreateMoreQuestions, promptContent);
                        }
                        else
                        {
                            await _createMoreQuestionsUseCase.ExecuteAsync(opts.CreateMoreQuestions, opts.SystemPrompt);
                        }
                    }
                });

            await parsingTask.ContinueWith(_ => CreateMonkey());

            // Fehlerbehandlung
            var parserResult = await parsingTask;

            if (parserResult.Tag == ParserResultType.NotParsed)
            {
                HandleParseError(((NotParsed<Options>)parserResult).Errors);
            }
            //CreateMonkey();
        }

        private async Task ViewResultSets()
        {
            Console.WriteLine("Result sets:\n");

            Console.WriteLine("  ID".PadRight(41) + "Value");
            Console.WriteLine(new string('-', 60));
            var resultSets = await _viewResultSetsUseCase.ExecuteAsync();
            foreach (var resultSet in resultSets)
            {
                Console.WriteLine($"  {resultSet.ResultSetId}   \"{resultSet.Value}\"");
            }
        }

        private void HandleParseError(IEnumerable<Error> errors)
        {
            // Die Standardhilfe wird automatisch angezeigt, wenn ein Fehler auftritt oder die Argumente falsch sind.
            // Hier kannst du benutzerdefinierte Logik hinzufügen, wenn nötig.
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
