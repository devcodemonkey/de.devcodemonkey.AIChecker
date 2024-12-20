﻿using CommandLine;
using de.devcodemonkey.AIChecker.AIChecker.Commands;
using de.devcodemonkey.AIChecker.CoreBusiness.DbModels;
using de.devcodemonkey.AIChecker.CoreBusiness.MarkDownExporterModels;
using de.devcodemonkey.AIChecker.CoreBusiness.Models;
using de.devcodemonkey.AIChecker.UseCases;
using de.devcodemonkey.AIChecker.UseCases.Interfaces;
using Spectre.Console;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

namespace de.devcodemonkey.AIChecker.AIChecker
{
    public class Application(
        IRecreateDatabaseUseCase recreateDatabaseUseCase,
        IImportQuestionAnswerUseCase importQuestionAnswerUseCase,
        IDeleteAllQuestionAnswerUseCase deleteAllQuestionAnswerUseCase,
        IDeleteResultSetUseCase deleteResultSetUseCase,
        ICreateMoreQuestionsUseCase createMoreQuestionsUseCase,
        IViewAverageTimeOfResultSetUseCase viewAverageTimeOfResultSetUseCase,
        IViewResultsOfResultSetUseCase viewResultsOfResultSetUseCase,
        IViewResultSetsUseCase viewResultSetsUseCase,
        ISendAndSaveApiRequestUseCase sendAPIRequestToLmStudioAndSaveToDbUseCase,
        IViewGpuUsageUseCase viewGpuUsageUseCase,
        IStartStopDatabaseUseCase startStopDatabaseUseCase,
        IBackupDatabaseUseCase backupDatabaseUseCase,
        IRestoreDatabaseUseCase restoreDatabaseUseCase,
        IAddModelUseCase addModelUseCase,
        IViewModels viewModels,
        ILoadModelUseCase loadModelUseCase,
        IUnloadModelUseCase unloadModelUseCase,
        ICreatePromptRatingUseCase createPromptRatingUseCase,
        IExportPromptRatingUseCase exportPromptRatingUseCase,
        IImportQuestionsFromResultsUseCase importQuestionsFromResultsUseCase,
        ISendQuestionsToLmsUseCase sendQuestionsToLmsUseCase,
        ICheckJsonFormatOfResultsUseCase checkJsonFormatOfResultsUseCase)
    {
        private const int MAX_DISTANCE = 58;
        private const int MONKEY_DISTANCE = 12;

        public async Task RunAsync(string[] args)
        {
            if (args.Length == 0)
            {
                await ViewResultSetsAsync();
                return;
            }

            var parsingTask = new Parser(config =>
            {
                config.HelpWriter = Console.Out;
            })
            .ParseArguments<DatabaseVerb, RankPromptVerb, SendToLmsVerb,
                ImportQuestionsVerb, ImportQuestionsFromResultsVerb, ViewResultSetsVerb,
                ViewAverageVerb, ViewResultsVerb, ViewProcessUsageVerb, DeleteResultSetVerb,
                DeleteAllQuestionsVerb, CreateMoreQuestionsVerb, ModelVerb, ExportPromptRankVerb,
                CheckJsonVerb, InfoVerb>(args)
            .MapResult(
                async (DatabaseVerb opts) => await StartStopDatabase(opts),

                async (RankPromptVerb opts) => await RankPrompt(opts),

                async (SendToLmsVerb opts) => await SendToLmsAsync(opts),

                async (ImportQuestionsVerb opts) => await importQuestionAnswerUseCase.ExecuteAsync(opts.Path, opts.Category),
                async (ImportQuestionsFromResultsVerb opts) => await ImportQuestionsFromResultsVerb(opts),

                async (ViewResultSetsVerb opts) => await ViewResultSetsAsync(),
                async (ViewAverageVerb opts) => await ViewAverageAsync(opts),
                async (ViewResultsVerb opts) => await ViewResultsAsync(opts),
                async (ViewProcessUsageVerb opts) => await ViewProcessUsageAsync(),

                async (DeleteResultSetVerb opts) => await DeleteResultSet(opts),
                async (DeleteAllQuestionsVerb opts) => await deleteAllQuestionAnswerUseCase.ExecuteAsync(),

                async (CreateMoreQuestionsVerb opts) => await CreateMoreQuestionsAsync(opts),
                async (ModelVerb opts) => await ManageModel(opts),

                async (ExportPromptRankVerb opts) => await ExportRank(opts),

                async (CheckJsonVerb opts) => await CheckJsonVerb(opts),

                async (InfoVerb opts) => await DisplayAppInfoAsync(),
                errs => Task.FromResult(0)
            );

            await parsingTask;
        }

        private async Task CheckJsonVerb(CheckJsonVerb opts)
        {
            await AnsiConsole.Status().StartAsync("Checking JSON format of results...", async ctx =>
            {
                var results = await checkJsonFormatOfResultsUseCase.ExecuteAsync(opts.ResultSet);
                if (opts.ShowOutput)
                {
                    var table = new Table();
                    table.AddColumn("ResultSet");
                    table.AddColumn("Model");
                    table.AddColumn("System Prompt");
                    table.AddColumn("Asked");
                    table.AddColumn("Message");
                    table.AddColumn("IsJson");
                    foreach (var result in results)
                    {
                        table.AddRow(
                            NewTextNoStyle(result.ResultSet.Value),
                            NewTextNoStyle(result.Model.Value),
                            NewTextNoStyle(result.SystemPrompt.Value),
                            NewTextNoStyle(result.Asked.ToString()),
                            NewTextNoStyle(result.Message),
                            NewTextNoStyle(result.IsJson ? "Yes" : "No"));
                    }
                    AnsiConsole.Write(new Rule("[yellow]Results[/]").RuleStyle("green"));
                    AnsiConsole.Write(table);
                }
                // Count the number of results that are not valid JSON format and that are valid JSON format and display the count
                var invalidJsonCount = results.Count(r => !r.IsJson);
                var validJsonCount = results.Count(r => r.IsJson);
                AnsiConsole.MarkupLine($"[yellow]Invalid JSON:[/] {invalidJsonCount}");
                AnsiConsole.MarkupLine($"[yellow]Valid JSON:[/] {validJsonCount}");
            });
        }

        private async Task DeleteResultSet(DeleteResultSetVerb opts)
        {
            await AnsiConsole.Status().StartAsync("Deleting result set...", async ctx =>
                await deleteResultSetUseCase.ExecuteAsync(opts.ResultSet)
            );
            AnsiConsole.MarkupLine("[green]Result set deleted![/]");
        }

        private async Task ImportQuestionsFromResultsVerb(ImportQuestionsFromResultsVerb opts)
            => await importQuestionsFromResultsUseCase.ExecuteAsync(opts.ResultSet, opts.Category);



        private async Task ExportRank(ExportPromptRankVerb opts)
        {
            await AnsiConsole.Status().StartAsync("Exporting prompt rating...", async ctx =>
            {
                //try parse the enum
                DataExportType fileType = opts.FileType?.ToLower() switch
                {
                    "pdf" => DataExportType.Pdf,
                    "markdown" => DataExportType.Markdown,
                    "html" => DataExportType.Html,
                    "docx" => DataExportType.Docx,
                    _ => DataExportType.Pdf
                };
                await exportPromptRatingUseCase.ExecuteAsync(opts.ResultSet, fileType, !opts.NotOpenFolder);
            }
            );
            AnsiConsole.MarkupLine("[green]Prompt rating exported![/]");
        }

        private async Task RankPrompt(RankPromptVerb opts)
        {
            int listNumber = 1;
            string? responseFormat = null;
            if (opts.ResponseFormat)
            {
                responseFormat = MultiLineInput("Response Format");
            }
            await createPromptRatingUseCase.ExecuteAsync(

                new PromptRatingUseCaseParams()
                {
                    ModelNames = opts.Models.ToArray(),
                    ResponseFormat = responseFormat,
                    MaxTokens = opts.MaxTokens,
                    ResultSet = opts.ResultSet,
                    Description = opts.Description,
                    PromptRequirements = opts.promptRequierements,
                    SystemPrompt = () =>
                    {
                        AnsiConsole.Write(new Rule($"[yellow]{listNumber}. Run[/]").RuleStyle("green"));
                        var str = MultiLineInput("System Prompt");
                        return str;
                    },
                    Message = () => MultiLineInput("Message"),
                    RatingReason = () => MultiLineInput("Rating Reason"),
                    Rating = () =>
                    {
                        int rank = 0;
                        do
                        {
                            rank = AnsiConsole.Ask<int>("Rating (1-10): ");
                        } while (rank < 1 || rank > 10);
                        return rank;
                    },
                    NewImprovement = () => AnsiConsole.Confirm($"New Improvement ({++listNumber}. Run ): ")
                },
                displayResult: (result) =>
                {
                    Table table = new();
                    table.AddColumn("Model");
                    table.AddColumn("Prompt Requierements");
                    table.AddColumn("System Prompt");
                    table.AddColumn("Message");
                    table.AddColumn("Max Tokens");
                    // delete format option with the 'new Style()'

                    table.AddRow(
                        NewTextNoStyle(result?.Model?.Value ?? string.Empty),
                        NewTextNoStyle(opts.promptRequierements),
                        NewTextNoStyle(result?.SystemPrompt?.Value ?? string.Empty),
                        NewTextNoStyle(result?.Message ?? string.Empty),
                        NewTextNoStyle(result?.MaxTokens?.ToString() ?? string.Empty));

                    AnsiConsole.Write(table);
                },
                statusHandler: (statusMessage, action) =>
                {
                    AnsiConsole.Status().Start(statusMessage, ctx => action());
                });
        }

        private string MultiLineInput(string text)
        {
            var inputLines = new List<string>();
            string fileContent = string.Empty;

            AnsiConsole.MarkupLine($"[bold yellow]Enter your multi-line input for [underline]{text}[/] (press [green]Ctrl+D[/] to finish, or [green]Ctrl+O[/] to open a text editor):[/]");

            while (true)
            {
                var keyInfo = Console.ReadKey(intercept: true);

                // Detect Ctrl+O for opening editor
                if (keyInfo.Key == ConsoleKey.O && (keyInfo.Modifiers & ConsoleModifiers.Control) != 0)
                {
                    fileContent = string.Join(Environment.NewLine, inputLines);
                    string tempFilePath = Path.GetTempFileName() + ".txt";
                    File.WriteAllText(tempFilePath, fileContent);

                    Process editorProcess = Process.Start("notepad.exe", tempFilePath);
                    editorProcess.WaitForExit();

                    // Load and display the content from the editor
                    fileContent = File.ReadAllText(tempFilePath);
                    AnsiConsole.MarkupLine("[bold cyan]\nImported content from editor:[/]");
                    Console.WriteLine(fileContent);

                    inputLines.Clear();
                    inputLines.Add(fileContent);

                    File.Delete(tempFilePath);

                    AnsiConsole.MarkupLine("[bold yellow]Press [green]Ctrl+D[/] to finish, or [green]Ctrl+O[/] to edit again.[/]");
                }
                // Detect Ctrl+D to end input
                else if (keyInfo.Key == ConsoleKey.D && (keyInfo.Modifiers & ConsoleModifiers.Control) != 0)
                {
                    break;
                }
                else
                {
                    Console.Write(keyInfo.KeyChar);
                    var line = keyInfo.KeyChar + Console.ReadLine();

                    if (!string.IsNullOrWhiteSpace(line))
                        inputLines.Add(line);
                }
            }

            AnsiConsole.MarkupLine("[bold green]Input completed.[/]");
            return string.Join(Environment.NewLine, inputLines);
        }

        private async Task ManageModel(ModelVerb opts)
        {
            switch (opts)
            {
                case { Load: true }:
                    var modelName = opts.ModelName;
                    if (string.IsNullOrEmpty(opts.ModelName))
                    {
                        var modelsAllProperties = await viewModels.ExecuteAsync();
                        var modelNames = modelsAllProperties.Select(m => m.Value).ToArray();
                        modelName = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .Title("Select a model to load")
                                .PageSize(10)
                                .AddChoices(modelNames)
                        );
                    }
                    //add loading screen
                    var success = false;
                    await AnsiConsole.Status().StartAsync("Loading model...", async ctx =>
                    {
                        success = await loadModelUseCase.ExecuteAsync(modelName);

                    });
                    if (success)
                        AnsiConsole.Markup($"[green]Model '{modelName}' loaded successfully.[/]");
                    else
                        AnsiConsole.Markup($"[red]Model '{modelName}' could not be loaded.[/]");
                    break;
                case { Unload: true }:
                    var successUnload = await unloadModelUseCase.ExecuteAsync();
                    if (successUnload)
                        AnsiConsole.Markup("[green]Model unloaded successfully.[/]");
                    else
                        AnsiConsole.Markup("[red]Model could not be unloaded.[/]");
                    break;
                case { Add: true }:
                    var value = AnsiConsole.Ask<string>("Enter the model name: ");
                    var basicModel = AnsiConsole.Ask<string>("Enter the basic models: ");
                    var link = AnsiConsole.Ask<string>("Enter the link: ");
                    var size = AnsiConsole.Ask<double>("Enter the size: ");
                    var model = new Model()
                    {
                        ModelId = Guid.NewGuid(),
                        Value = value,
                        BaseModels = basicModel,
                        Link = link,
                        Size = size
                    };
                    await addModelUseCase.ExecuteAsync(model);
                    break;
                default:
                    var models = await viewModels.ExecuteAsync();
                    var table = new Table();
                    table.AddColumn("Model");
                    table.AddColumn("Basic Models");
                    table.AddColumn("Link");
                    table.AddColumn("Size");
                    foreach (var m in models)
                    {
                        // delete format option with the 'new Style()'                        
                        table.AddRow(
                            NewTextNoStyle(m.Value),
                            NewTextNoStyle(m.BaseModels ?? ""),
                            NewTextNoStyle(m.Link ?? ""),
                            NewTextNoStyle(m.Size.ToString()));
                    }
                    AnsiConsole.Write(table);
                    break;
            }
        }

        private async Task StartStopDatabase(DatabaseVerb opts)
        {

            if (opts.Restore)
            {
                var restoreSuccess = restoreDatabaseUseCase.Execute(opts.Branch);
                if (restoreSuccess)
                    AnsiConsole.Markup("[green]Database restored successfully.[/]");
                else
                    AnsiConsole.Markup("[red]Database restore failed.[/]");
                return;
            }
            if (opts.Backup)
            {
                var backupSuccess = backupDatabaseUseCase.Execute();
                if (backupSuccess)
                    AnsiConsole.Markup("[green]Database backup successful.[/]");
                else
                    AnsiConsole.Markup("[red]Database backup failed.[/]");
                return;
            }
            if (opts.RecreateDatabase)
            {
                await RecreateDatabaseAsync(opts);
                return;
            }

            if (opts.Stop)
                await startStopDatabaseUseCase.ExecuteAsync(false);
            else
                await startStopDatabaseUseCase.ExecuteAsync(true);
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

        private async Task RecreateDatabaseAsync(DatabaseVerb opts)
        {
            if (!opts.Force)
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
            }
            await AnsiConsole.Status().StartAsync("Recreating database...", async ctx =>
                {
                    await recreateDatabaseUseCase.ExecuteAysnc();
                });

            AnsiConsole.MarkupLine("[green]Database recreated![/]");
        }

        private async Task ViewProcessUsageAsync()
        {
            await AnsiConsole.Status().StartAsync("Loading GPU usage...", async ctx =>
            {
                await viewGpuUsageUseCase.ExecuteAsync();
            });
        }

        private async Task SendToLmsAsync(SendToLmsVerb opts)
        {
            if (string.IsNullOrWhiteSpace(opts.QuestionCategory) && string.IsNullOrWhiteSpace(opts.UserMessage))
                throw new ArgumentException("Either 'message' or 'questionCategory' must be provided.");
            if (opts.ResponseFormat != null && opts.ResponseFormat.ToLower().Equals("ask"))
                opts.ResponseFormat = MultiLineInput("Response Format");
            if (opts.SystemPrompt != null && opts.SystemPrompt.ToLower().Equals("ask"))
                opts.SystemPrompt = MultiLineInput("System Prompt");
            if (opts.UserMessage != null && opts.UserMessage.ToLower().Equals("ask"))
                opts.UserMessage = MultiLineInput("User Message");

            var isOpenAiModel = opts.Model.ToLower().Contains("gpt");
            await AnsiConsole.Status().StartAsync($"Sending API request to {(isOpenAiModel ? "OpenAi Api" : "LmStudio")} and saving to db...", async ctx =>
            {
                if (string.IsNullOrEmpty(opts.QuestionCategory))
                    await sendAPIRequestToLmStudioAndSaveToDbUseCase.ExecuteAsync(opts);
                else
                    await sendQuestionsToLmsUseCase.ExecuteAsync(opts, progressAction: (progressMetrics) =>
                    {
                        // Display progress metrics
                        var table = new Table()
                            .Border(TableBorder.Rounded)
                            .Title("[yellow bold]Progress Metrics[/]")
                            .AddColumn("[bold yellow]Metric[/]")
                            .AddColumn("[bold yellow]Value[/]")
                            .AddRow("[green]Questions[/]", $"[bold]{progressMetrics.QuestionsCounter}/{progressMetrics.QuestionsCount}[/]")
                            .AddRow("[green]Answers[/]", $"[bold]{progressMetrics.AnswersCounter}/{progressMetrics.AnswersCount}[/]")
                            .AddRow("[green]Total[/]", $"[bold]{progressMetrics.TotalCounter}/{progressMetrics.QuestionsCount * progressMetrics.AnswersCount}[/]")
                            .AddRow("[green]Running Time[/]", $"[bold]{progressMetrics.RunningTime.ToString(@"hh\:mm\:ss")}[/]")
                            .AddRow("[green]Estimated Time to Finish[/]", $"[bold]{progressMetrics.CalulationTime.ToString(@"hh\:mm\:ss")}[/]");

                        AnsiConsole.Write(table);
                    });
            });
        }


        private async Task ViewResultSetsAsync()
        {
            await AnsiConsole.Status().StartAsync("Loading result sets...", async ctx =>
            {
                var table = new Table();
                table.AddColumn(new TableColumn("[bold yellow]ID[/]").Centered());
                table.AddColumn(new TableColumn("[bold yellow]Value[/]").LeftAligned());
                table.AddColumn(new TableColumn("[bold yellow]Average Time (s)[/]").Centered());

                var resultSets = await viewResultSetsUseCase.ExecuteAsync();

                resultSets = resultSets.OrderBy(r => r.Item1.Value);
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
            var result = await viewAverageTimeOfResultSetUseCase.ExecuteAsync(opts.ResultSet);
            Console.WriteLine($"The average time of the API request for result set '{opts.ResultSet}' is {result.TotalSeconds} seconds.");
        }

        private async Task ViewResultsAsync(ViewResultsVerb opts)
        {
            await AnsiConsole.Status().StartAsync("Loading results...", async ctx =>
            {
                var results = await viewResultsOfResultSetUseCase.ExecuteAsync(opts.ResultSet);
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
                        NewTextNoStyle(opts.FormatTable ? Truncate(result.ResultSet.Value, 50) : result.ResultSet.Value),
                        NewTextNoStyle(opts.FormatTable ? Truncate(result.Model.Value, 50) : result.Model.Value),
                        NewTextNoStyle(opts.FormatTable ? Truncate(result.SystemPrompt.Value, 50) : result.SystemPrompt.Value),
                        NewTextNoStyle(opts.FormatTable ? Truncate(result.Asked.ToString(), 50) : result.Asked.ToString()),
                        NewTextNoStyle(opts.FormatTable ? Truncate(result.Message, 50) : result.Message),
                        NewTextNoStyle(opts.FormatTable ? Truncate(result.Temperature.ToString(), 50) : result.Temperature.ToString()),
                        NewTextNoStyle(opts.FormatTable ? Truncate(result.MaxTokens.ToString(), 50) : result.MaxTokens.ToString()),
                        NewTextNoStyle(opts.FormatTable ? Truncate(result.PromptTokens.ToString(), 50) : result.PromptTokens.ToString()),
                        NewTextNoStyle(opts.FormatTable ? Truncate(result.CompletionTokens.ToString(), 50) : result.CompletionTokens.ToString()),
                        NewTextNoStyle(opts.FormatTable ? Truncate(result.TotalTokens.ToString(), 50) : result.TotalTokens.ToString()));
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
            opts.ResponseFormat = MultiLineInput("Response Format");
            await AnsiConsole.Status().StartAsync("Creating more questions...", async ctx =>
                await createMoreQuestionsUseCase.ExecuteAsync(opts));
        }

        private Text NewTextNoStyle(string text) => new Text(text, new Style());

    }
}
