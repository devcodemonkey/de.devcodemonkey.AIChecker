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
                await ViewResultSetsAsync();
                CreateMonkey();
                return;
            }
            //args = ["--importQuestionAnswer", "C:\\Users\\d-hoe\\source\\repos\\masterarbeit\\AIChecker\\Plugins\\JsonDeserializer\\Example.json"];
            //args = ["--viewAverage", "Test set"];
            //args = ["--deleteAllEntityQuestionAnswer"];
            //args = ["--importQuestionAnswer", "C:\\Users\\d-hoe\\source\\repos\\masterarbeit.wiki\\06_00_00-Ticketexport\\FAQs\\FAQ-Outlook.json"];
            //args = ["--createMoreQuestions", "Test set",
            //    "path:C:\\Users\\d-hoe\\source\\repos\\masterarbeit\\AIChecker\\AIChecker\\examples\\system_promt.txt"];
            var parsingTask = Parser.Default.ParseArguments<ImportQuestionsVerb, ViewResultSetsVerb, ViewAverageVerb, DeleteAllQuestionsVerb, CreateMoreQuestionsVerb>(args)
                .MapResult(
                    async (ImportQuestionsVerb opts) =>
                    {
                        await _importQuestionAnswerUseCase.ExecuteAsnc(opts.Path);
                    },
                    async (ViewResultSetsVerb opts) =>
                    {
                        await ViewResultSetsAsync();
                    },
                    async (ViewAverageVerb opts) =>
                    {
                        var result = await _viewAvarageTimeOfResultSetUseCase.ExecuteAsync(opts.ResultSet);
                        Console.WriteLine($"The average time of the API request of the result set '{opts.ResultSet}' is {result}.");
                    },
                    async (DeleteAllQuestionsVerb opts) =>
                    {
                        await _deleteAllQuestionAnswerUseCase.ExecuteAsync();
                    },
                    async (CreateMoreQuestionsVerb opts) =>
                    {
                        if (opts.SystemPrompt.StartsWith("path:"))
                        {
                            var filePath = opts.SystemPrompt.Substring(5);
                            var promptContent = File.ReadAllText(filePath);
                            await _createMoreQuestionsUseCase.ExecuteAsync(opts.ResultSet, promptContent);
                        }
                        else
                        {
                            await _createMoreQuestionsUseCase.ExecuteAsync(opts.ResultSet, opts.SystemPrompt);
                        }
                    },
                    errs => Task.FromResult(0)); // Fehlerbehandlung

            await parsingTask.ContinueWith(_ => CreateMonkey());
        }

        // Verbs Definition
        [Verb("importQuestions", HelpText = "Imports Questions and Answers to the db.")]
        public class ImportQuestionsVerb
        {
            [Option('p', "path", Required = true, HelpText = "Path to the file with Questions and Answers.")]
            public string Path { get; set; }
        }

        [Verb("viewAverage", HelpText = "View the average time of the API request of a 'result set'.")]
        public class ViewAverageVerb
        {
            [Option('r', "ResultSet", Required = true, HelpText = "The result set name.")]
            //[Value(0, MetaName = "ResultSet", Required = true, HelpText = "The result set name.")]
            public string ResultSet { get; set; }
        }

        [Verb("viewResultSets", HelpText = "View all result sets.")]
        public class ViewResultSetsVerb { }

        [Verb("deleteAllQuestions", HelpText = "Deletes all Questions and Answers from the db.")]
        public class DeleteAllQuestionsVerb { }

        [Verb("createMoreQuestions", HelpText = "Creates more questions under the 'system prompt' and saves them under the result 'set name'.")]
        public class CreateMoreQuestionsVerb
        {
            [Option('r', "resultSet", Required = true, HelpText = "The result set name.")]
            public string ResultSet { get; set; }

            [Option('s', "systemPrompt", Required = true, HelpText = "The system prompt or path (path:<file-path>) for creating questions.")]
            public string SystemPrompt { get; set; }
        }

        private async Task ViewResultSetsAsync()
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
