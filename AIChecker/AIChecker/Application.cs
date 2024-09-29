using CommandLine;
using de.devcodemonkey.AIChecker.AIChecker.Commands;
using de.devcodemonkey.AIChecker.UseCases.Interfaces;
using Spectre.Console;
using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace de.devcodemonkey.AIChecker.AIChecker
{
    public class Application
    {
        private const int MAX_DISTANCE = 58;
        private const int MONKEY_DISTANCE = 12;

        private readonly IRecreateDatabaseUseCase _recreateDatabaseUseCase;
        private readonly IImportQuestionAnswerUseCase _importQuestionAnswerUseCase;
        private readonly IDeleteAllQuestionAnswerUseCase _deleteAllQuestionAnswerUseCase;
        private readonly ICreateMoreQuestionsUseCase _createMoreQuestionsUseCase;
        private readonly IViewAverageTimeOfResultSetUseCase _viewAverageTimeOfResultSetUseCase;
        private readonly IViewResultSetsUseCase _viewResultSetsUseCase;
        private readonly ISendAPIRequestToLmStudioAndSaveToDbUseCase _sendAPIRequestToLmStudioAndSaveToDbUseCase;
        private readonly IDeleteResultSetUseCase _deleteResultSetUseCase;
        private readonly IViewResultsOfResultSetUseCase _viewResultsOfResultSetUseCase;

        private readonly IViewGpuUsageUseCase _viewGpuUsageUseCase;

        public Application(
            IRecreateDatabaseUseCase recreateDatabaseUseCase,
            IImportQuestionAnswerUseCase importQuestionAnswerUseCase,
            IDeleteAllQuestionAnswerUseCase deleteAllQuestionAnswerUseCase,
            IDeleteResultSetUseCase deleteResultSetUseCase,
            ICreateMoreQuestionsUseCase createMoreQuestionsUseCase,
            IViewAverageTimeOfResultSetUseCase viewAverageTimeOfResultSetUseCase,
            IViewResultsOfResultSetUseCase viewResultsOfResultSetUseCase,
            IViewResultSetsUseCase viewResultSetsUseCase,
            ISendAPIRequestToLmStudioAndSaveToDbUseCase sendAPIRequestToLmStudioAndSaveToDbUseCase,
            IViewGpuUsageUseCase viewGpuUsageUseCase)
        {
            _recreateDatabaseUseCase = recreateDatabaseUseCase;
            _importQuestionAnswerUseCase = importQuestionAnswerUseCase;
            _deleteAllQuestionAnswerUseCase = deleteAllQuestionAnswerUseCase;
            _deleteResultSetUseCase = deleteResultSetUseCase;
            _createMoreQuestionsUseCase = createMoreQuestionsUseCase;
            _viewAverageTimeOfResultSetUseCase = viewAverageTimeOfResultSetUseCase;
            _viewResultSetsUseCase = viewResultSetsUseCase;
            _viewResultsOfResultSetUseCase = viewResultsOfResultSetUseCase;
            _sendAPIRequestToLmStudioAndSaveToDbUseCase = sendAPIRequestToLmStudioAndSaveToDbUseCase;
            _viewGpuUsageUseCase = viewGpuUsageUseCase;
        }

        public async Task RunAsync(string[] args)
        {
            //args = ["sendToLms", "-m", "Schreib mir ein Gedicht mit 100 Zeilen", "-s", "Du achtest darauf, dass sich alles reimt", "-r", "Requesttime check: | model: Phi-3.5-mini-instruct", "-c", "5", "-i", "5"];
            // args = ["sendToLms", "-s", "", "-r" ,"Test result set", "-m", "write me a poem over 10 lines"];
            //args = ["deleteResultSet", "-r", "cbc94e4a-868a-4751-aec1-9800dfbdcf08"];
            //args = ["viewResults", "-r", "7d26beed-3e04-4f7f-adb4-19bceca49503"];
            // args = ["viewUsedGpu"];
            //args = ["info"];
            //args = ["recreateDatabase"];
            // args = ["importQuestions", "-p", "/home/david/masterarbeit.wiki/06_00_00-Ticketexport/FAQs/FAQ-Outlook.json"];

            if (args.Length == 0)
            {
                await ViewResultSetsAsync();
                return;
            }

            var parsingTask = new Parser(config =>
            {
                config.HelpWriter = Console.Out;
            })
            .ParseArguments<InfoVerb, RecreateDatabaseVerb, ImportQuestionsVerb, ViewResultSetsVerb,
                            ViewAverageVerb, ViewResultsVerb, ViewUsedGpuVerb, DeleteAllQuestionsVerb,
                            DeleteResultSetVerb, CreateMoreQuestionsVerb, SendToLmsVerb>(args)
            .MapResult(
                async (InfoVerb opts) => await DisplayAppInfoAsync(),
                async (RecreateDatabaseVerb opts) => await RecreateDatabaseAsync(),
                async (ViewUsedGpuVerb opts) => await ViewUsedGpuAsync(),
                async (SendToLmsVerb opts) => await SendToLmsAsync(opts),
                async (ImportQuestionsVerb opts) => await _importQuestionAnswerUseCase.ExecuteAsync(opts.Path),
                async (ViewResultSetsVerb opts) => await ViewResultSetsAsync(),
                async (ViewAverageVerb opts) => await ViewAverageAsync(opts),
                async (ViewResultsVerb opts) => await ViewResultsAsync(opts),
                async (DeleteResultSetVerb opts) => await _deleteResultSetUseCase.ExecuteAsync(opts.ResultSet),
                async (DeleteAllQuestionsVerb opts) => await _deleteAllQuestionAnswerUseCase.ExecuteAsync(),
                async (CreateMoreQuestionsVerb opts) => await CreateMoreQuestionsAsync(opts),
                errs => Task.FromResult(0)
            );

            await parsingTask;
        }

        private Task DisplayAppInfoAsync()
        {
            var version = Assembly.GetExecutingAssembly()
                                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
            var repo = Assembly.GetExecutingAssembly()
                                .GetCustomAttribute<AssemblyMetadataAttribute>()?.Value;

            AnsiConsole.Write(new FigletText("AiChecker").Color(Color.Green));

            AnsiConsole.Write(new Rule("[yellow]Author Info[/]").RuleStyle("green"));
            AnsiConsole.MarkupLine("[bold yellow]Author:[/]     [green]David Höll[/]");
            AnsiConsole.MarkupLine("[bold yellow]Email:[/]      [blue]info@hl-dev.de[/]");
            AnsiConsole.MarkupLine("[bold yellow]Website:[/]    [blue]https://devcodemonkey.de[/]");

            AnsiConsole.Write(new Rule("[yellow]Project Info[/]").RuleStyle("green"));
            AnsiConsole.MarkupLine($"[bold yellow]Version:[/]    [green]{version}[/]");
            AnsiConsole.MarkupLine($"[bold yellow]Repository:[/] [blue]{repo}[/]");
            AnsiConsole.MarkupLine("[bold yellow]Updates:[/]    [blue]https://devcodemonkey.de/AiChecker[/]");
            AnsiConsole.MarkupLine("[bold yellow]License:[/]    [green]MIT[/]");
            AnsiConsole.MarkupLine($"[bold yellow]Copyright[/]   [green]© 2024 David Höll[/]");

            return Task.CompletedTask;
        }

        private async Task RecreateDatabaseAsync()
        {
            AnsiConsole.MarkupLine("[red]Warning![/] All data will be lost!");
            string confirm;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                confirm = AnsiConsole.Ask<string>("Type 'delete all data' to confirm: ");
            else
            {
                System.Console.WriteLine("Type 'delete all data' to confirm: ");
                confirm = Console.ReadLine() ?? string.Empty;
            }

            if (confirm != "delete all data")
            {
                AnsiConsole.MarkupLine("[red]Aborted![/]");
                return;
            }

            await AnsiConsole.Status().StartAsync("Recreating database...", async ctx =>
            {
                await _recreateDatabaseUseCase.ExecuteAysnc();
            });

            AnsiConsole.MarkupLine("[green]Database recreated![/]");
        }

        private async Task ViewUsedGpuAsync()
        {
            await AnsiConsole.Status().StartAsync("Loading GPU usage...", async ctx =>
            {
                await _viewGpuUsageUseCase.ExecuteAsync();
            });
        }

        private async Task SendToLmsAsync(SendToLmsVerb opts)
        {
            await AnsiConsole.Status().StartAsync("Sending API request to LmStudio and saving to db...", async ctx =>
            {
                await _sendAPIRequestToLmStudioAndSaveToDbUseCase.ExecuteAsync(
                    opts.Message,
                    opts.SystemPrompt,
                    opts.ResultSet,
                    opts.RequestCount,
                    opts.MaxTokens,
                    opts.Temperature,
                    saveInterval: opts.SaveInterval,
                    saveProcessUsage: opts.SaveProcessUsage,
                    model: opts.Model,
                    source: opts.Source,
                    environmentTokenName: opts.EnvironmentTokenName
                );
            });
        }

        private async Task ViewResultSetsAsync()
        {
            await AnsiConsole.Status().StartAsync("Loading result sets...", async ctx =>
            {
                var table = new Table();
                table.AddColumn(new TableColumn("[bold yellow]ID[/]").Centered());
                table.AddColumn(new TableColumn("[bold yellow]Value[/]").Centered());
                table.AddColumn(new TableColumn("[bold yellow]Average Time (s)[/]").Centered());

                var resultSets = await _viewResultSetsUseCase.ExecuteAsync();
                foreach (var resultSet in resultSets)
                {
                    table.AddRow(
                        resultSet.Item1.ResultSetId.ToString(),
                        resultSet.Item1.Value,
                        resultSet.Item2.TotalSeconds.ToString("F2")  // Format to two decimal places for consistency
                    );
                }

                AnsiConsole.Write(new Rule("[yellow]Result sets:[/]").RuleStyle("green"));

                AnsiConsole.Write(table);
            });
        }

        private async Task ViewAverageAsync(ViewAverageVerb opts)
        {
            var result = await _viewAverageTimeOfResultSetUseCase.ExecuteAsync(opts.ResultSet);
            Console.WriteLine($"The average time of the API request for result set '{opts.ResultSet}' is {result.TotalSeconds} seconds.");
        }

        private async Task ViewResultsAsync(ViewResultsVerb opts)
        {
            await AnsiConsole.Status().StartAsync("Loading results...", async ctx =>
            {
                var results = await _viewResultsOfResultSetUseCase.ExecuteAsync(opts.ResultSet);
                var table = new Table();
                table.AddColumn("ResultSet");
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
                        new Text(opts.FormatTable ? Truncate(result.ResultSet.Value, 50) : result.ResultSet.Value, new Style()),
                        new Text(opts.FormatTable ? Truncate(result.Model.Value, 50) : result.Model.Value, new Style()),
                        new Text(opts.FormatTable ? Truncate(result.SystemPrompt.Value, 50) : result.SystemPrompt.Value, new Style()),
                        new Text(opts.FormatTable ? Truncate(result.Asked.ToString(), 50) : result.Asked.ToString(), new Style()),
                        new Text(opts.FormatTable ? Truncate(result.Message, 50) : result.Message, new Style()),
                        new Text(opts.FormatTable ? Truncate(result.Temperature.ToString(), 50) : result.Temperature.ToString(), new Style()),
                        new Text(opts.FormatTable ? Truncate(result.MaxTokens.ToString(), 50) : result.MaxTokens.ToString(), new Style()),
                        new Text(opts.FormatTable ? Truncate(result.PromptTokens.ToString(), 50) : result.PromptTokens.ToString(), new Style()),
                        new Text(opts.FormatTable ? Truncate(result.CompletionTokens.ToString(), 50) : result.CompletionTokens.ToString(), new Style()),
                        new Text(opts.FormatTable ? Truncate(result.TotalTokens.ToString(), 50) : result.TotalTokens.ToString(), new Style())
                     );
                }

                AnsiConsole.Write(new Rule("[yellow]Result[/]").RuleStyle("green"));
                AnsiConsole.Write(table);
            });
        }

        private string Truncate(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            return value.Length <= maxLength ? value : value.Substring(0, maxLength) + "...";
        }

        private async Task CreateMoreQuestionsAsync(CreateMoreQuestionsVerb opts)
        {
            await AnsiConsole.Status().StartAsync("Creating more questions...", async ctx =>
            {
                if (opts.SystemPrompt.StartsWith("path:"))
                {
                    var filePath = opts.SystemPrompt.Substring(5);
                    var promptContent = File.ReadAllText(filePath);
                    await _createMoreQuestionsUseCase.ExecuteAsync(opts.ResultSet, promptContent, opts.MaxTokens, opts.Temperature);
                }
                else
                {
                    await _createMoreQuestionsUseCase.ExecuteAsync(opts.ResultSet, opts.SystemPrompt);
                }
            });
        }
    }
}
