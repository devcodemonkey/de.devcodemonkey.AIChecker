using CommandLine;
using de.devcodemonkey.AIChecker.AIChecker.Commands;
using de.devcodemonkey.AIChecker.UseCases.Interfaces;
using Spectre.Console;


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
            //args = ["sendToLMS", "-m", "Schreib mir ein Gedicht mit 100 Zeilen", "-s", "Du achtest darauf, dass sich alles reimt", "-r", "Requesttime check: | model: Phi-3.5-mini-instruct", "-c", "5", "-i", "5"];
            //args = ["deleteResultSet", "-r", "cbc94e4a-868a-4751-aec1-9800dfbdcf08"];
            //args = ["viewResults", "-r", "7d26beed-3e04-4f7f-adb4-19bceca49503"];
            //args = ["view-used-gpu"];
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
                ViewUsedGpuVerb,
                DeleteAllQuestionsVerb,
                DeleteResultSetVerb,
                CreateMoreQuestionsVerb,
                SendToLMSVerb>(args)
                .MapResult(
                    async (ViewUsedGpuVerb opts) =>
                    {

                    },
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
                                    ops.Temperature,
                                    saveInterval: ops.SaveInterval,
                                    saveProcessUsage: ops.SaveProcessUsage
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
                                table.AddColumn("ResultSet");
                                //table.AddColumn("RequestObject");
                                //table.AddColumn("RequestReason");
                                table.AddColumn("Model");
                                table.AddColumn("System Prompt");
                                table.AddColumn("Asked");
                                table.AddColumn("Message");
                                table.AddColumn("Temp");
                                table.AddColumn("MaxTo");
                                table.AddColumn("PoTo");
                                table.AddColumn("CoTo");
                                table.AddColumn("Too");
                                foreach (var result in results)
                                {
                                    table.AddRow(
                                        // Disable markup
                                        new Text(result.ResultSet.Value, new Style()),
                                        //new Text(result.RequestObject.Value, new Style()),   
                                        //new Text(result.RequestReason.Value, new Style()),   
                                        new Text(result.Model.Value, new Style()),
                                        new Text(result.SystemPromt.Value, new Style()),
                                        new Text(result.Asked.ToString(), new Style()),
                                        new Text(result.Message, new Style()),
                                        new Text(result.Temperature.ToString(), new Style()),
                                        new Text(result.MaxTokens.ToString(), new Style()),
                                        new Text(result.PromtTokens.ToString(), new Style()),
                                        new Text(result.CompletionTokens.ToString(), new Style()),
                                        new Text(result.TotalTokens.ToString(), new Style()));
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
                                    await _createMoreQuestionsUseCase.ExecuteAsync(opts.ResultSet,
                                        promptContent,
                                        maxTokens: opts.MaxTokens,
                                        temperture: opts.Temperature);
                                }
                                else
                                    await _createMoreQuestionsUseCase.ExecuteAsync(opts.ResultSet, opts.SystemPrompt);
                            }),
                    errs => Task.FromResult(0)); // Fehlerbehandlung

            await parsingTask;
            //await parsingTask.ContinueWith(_ => CreateMonkey());
        }

        private async Task ViewResultSetsAsync()
        {
            await AnsiConsole
                .Status()
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
    }
}
