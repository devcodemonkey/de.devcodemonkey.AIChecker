using de.devcodemonkey.AIChecker.UseCases.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using Microsoft.Extensions.Options;
using Spectre.Console;
using de.devcodemonkey.AIChecker.CoreBusiness.Models;


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
        private readonly ISendAPIRequestToLmStudioAndSaveToDbUseCase _sendAPIRequestToLmStudioAndSaveToDbUseCase;
        private readonly IDeleteResultSetUseCase _deleteResultSetUseCase;
        private readonly IViewResultsOfResultSetUseCase _viewResultsOfResultSetUseCase;

        public Application(IImportQuestionAnswerUseCase importQuestionAnswerUseCase,
            IDeleteAllQuestionAnswerUseCase deleteAllQuestionAnswerUseCase,
            IDeleteResultSetUseCase deleteResultSetUseCase,
            ICreateMoreQuestionsUseCase createMoreQuestionsUseCase,
            IViewAvarageTimeOfResultSetUseCase viewAvarageTimeOfResultSetUseCase,
            IViewResultsOfResultSetUseCase viewResultsOfResultSetUseCase,
            IViewResultSetsUseCase viewResultSetsUseCase,
            ISendAPIRequestToLmStudioAndSaveToDbUseCase sendAPIRequestToLmStudioAndSaveToDbUseCase)
        {
            _importQuestionAnswerUseCase = importQuestionAnswerUseCase;
            _deleteAllQuestionAnswerUseCase = deleteAllQuestionAnswerUseCase;
            _deleteResultSetUseCase = deleteResultSetUseCase;
            _createMoreQuestionsUseCase = createMoreQuestionsUseCase;
            _viewAvarageTimeOfResultSetUseCase = viewAvarageTimeOfResultSetUseCase;
            _viewResultSetsUseCase = viewResultSetsUseCase;
            _viewResultsOfResultSetUseCase = viewResultsOfResultSetUseCase;
            _sendAPIRequestToLmStudioAndSaveToDbUseCase = sendAPIRequestToLmStudioAndSaveToDbUseCase;
        }

        public async Task RunAsync(string[] args)
        {
            args = ["sendToLMS", "-m", "Schreib mir ein Gedicht", "-s", "Du achtest darauf, dass sich alles reimt", "-r", "Requesttime check: | model: Phi-3.5-mini-instruct", "-c", "1"];
            //args = ["deleteResultSet", "-r", "cbc94e4a-868a-4751-aec1-9800dfbdcf08"];
            //args = ["viewResults", "-r", "bfdbb285-372c-4ff3-a646-bbd8969fdee8"];
            if (args.Length == 0)
            {
                await ViewResultSetsAsync();
                //CreateMonkey();
                return;
            }

            var parsingTask = new Parser(config =>
            {
                //CreateMonkey();
                config.HelpWriter = Console.Out;

            }).ParseArguments<ImportQuestionsVerb,
                ViewResultSetsVerb,
                ViewAverageVerb,
                ViewResultsVerb,
                DeleteAllQuestionsVerb,
                DeleteResultSetVerb,
                CreateMoreQuestionsVerb,
                SendToLMSVerb>(args)
                .MapResult(
                    async (SendToLMSVerb ops) =>
                        await AnsiConsole.Status()
                            .StartAsync("Sending API request to LmStudio and saving to db...", async ctx =>
                            {
                                await _sendAPIRequestToLmStudioAndSaveToDbUseCase.ExecuteAsync(
                                    ops.Message,
                                    ops.SystemPrompt,
                                    ops.ResultSet,
                                    ops.RequestCount,
                                    ops.MaxTokens,
                                    ops.Temperature
                                );
                            }),
                    async (ImportQuestionsVerb opts)
                        => await _importQuestionAnswerUseCase.ExecuteAsnc(opts.Path),
                    async (ViewResultSetsVerb opts)
                        => await ViewResultSetsAsync(),
                    async (ViewAverageVerb opts) =>
                    {
                        var result = await _viewAvarageTimeOfResultSetUseCase.ExecuteAsync(opts.ResultSet);
                        Console.WriteLine($"The average time of the API request of the result set '{opts.ResultSet}' is {result.TotalSeconds} seconds.");
                    },
                    async (ViewResultsVerb opts) =>
                    {
                        await AnsiConsole.Status()
                            .StartAsync("Loading results...", async ctx =>
                            {
                                var results = await _viewResultsOfResultSetUseCase.ExecuteAsync(opts.ResultSet);
                                var table = new Table();
                                table.AddColumn("Asked");
                                table.AddColumn("Message");
                                table.AddColumn("System Prompt");
                                table.AddColumn("Temperature");
                                foreach (var result in results)
                                {
                                    table.AddRow(
                                       new Text(result.Asked.ToString(), new Style()),        // Disable markup
                                       new Text(result.Message, new Style()),                 // Disable markup
                                       new Text(result.SystemPromt.Value, new Style()),       // Disable markup
                                       new Text(result.Temperture.ToString(), new Style())    // Disable markup
                                       );
                                }
                                AnsiConsole.WriteLine("Results:");
                                AnsiConsole.Write(table);
                            });
                    },
                    async (DeleteResultSetVerb opts)
                        => await _deleteResultSetUseCase.ExecuteAsync(opts.ResultSet),
                    async (DeleteAllQuestionsVerb opts)
                        => await _deleteAllQuestionAnswerUseCase.ExecuteAsync(),
                    async (CreateMoreQuestionsVerb opts) =>
                        await AnsiConsole.Status()
                            .StartAsync("Creating more questions...", async ctx =>
                            {
                                if (opts.SystemPrompt.StartsWith("path:"))
                                {
                                    var filePath = opts.SystemPrompt.Substring(5);
                                    var promptContent = File.ReadAllText(filePath);
                                    await _createMoreQuestionsUseCase.ExecuteAsync(opts.ResultSet, promptContent);
                                }
                                else
                                    await _createMoreQuestionsUseCase.ExecuteAsync(opts.ResultSet, opts.SystemPrompt);
                            }),
                    errs => Task.FromResult(0)); // Fehlerbehandlung

            await parsingTask;
            //await parsingTask.ContinueWith(_ => CreateMonkey());
        }

        // Verbs Definition
        [Verb("sendToLMS", HelpText = "Sends an API request to the LmStudio and saves the result to the db.")]
        public class SendToLMSVerb
        {
            [Option('m', "message", Required = true, HelpText = "The user message.")]
            public string Message { get; set; }
            [Option('s', "systemPrompt", Required = true, HelpText = "The system prompt.")]
            public string SystemPrompt { get; set; }
            [Option('r', "resultSet", Required = true, HelpText = "The result set name.")]
            public string ResultSet { get; set; }
            [Option('c', "requestCount", Default = 1, HelpText = "The number of requests.")]
            public int RequestCount { get; set; }
            [Option('t', "maxTokens", Default = -1, HelpText = "The maximum number of tokens.")]
            public int MaxTokens { get; set; }
            [Option('p', "temperature", Default = 0.7, HelpText = "The temperature.")]
            public double Temperature { get; set; }
        }


        [Verb("importQuestions", HelpText = "Imports Questions and Answers to the db.")]
        public class ImportQuestionsVerb
        {
            [Option('p', "path", Required = true, HelpText = "Path to the file with Questions and Answers.")]
            public string Path { get; set; }
        }

        [Verb("viewResults", HelpText = "View Results of a result set")]
        public class ViewResultsVerb
        {
            [Option('r', "ResultSet", Required = true, HelpText = "The result set name.")]
            public string ResultSet { get; set; }
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

        [Verb("deleteResultSet", HelpText = "Deletes a 'result set'.")]
        public class DeleteResultSetVerb
        {
            [Option('r', "ResultSet", Required = true, HelpText = "The result set name.")]
            public string ResultSet { get; set; }
        }

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
            await AnsiConsole
                .Status()
                .Spinner(Spinner.Known.Star)
                .StartAsync("Loading result sets...", async ctx =>
                {
                    var table = new Table();
                    table.AddColumn("ID");
                    table.AddColumn("Value");
                    table.AddColumn("Average Time");

                    var resultSets = await _viewResultSetsUseCase.ExecuteAsync();
                    foreach (var resultSet in resultSets)
                    {
                        table.AddRow(resultSet.Item1.ResultSetId.ToString(), resultSet.Item1.Value, resultSet.Item2.TotalSeconds.ToString());
                    }
                    AnsiConsole.WriteLine("Result sets:");
                    AnsiConsole.Write(table);
                });



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
