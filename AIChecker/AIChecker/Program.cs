using de.devcodemonkey.AIChecker.CoreBusiness.Models;
using de.devcodemonkey.AIChecker.DataSource.APIRequester;
using de.devcodemonkey.AIChecker.DataSource.SystemMonitor;
using de.devcodemonkey.AIChecker.DataStore.PostgreSqlEF;
using de.devcodemonkey.AIChecker.Importer.JsonDeserializer;
using de.devcodemonkey.AIChecker.MarkdownExporter;
using de.devcodemonkey.AIChecker.UseCases;
using de.devcodemonkey.AIChecker.UseCases.Interfaces;
using de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;
using LmsWrapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;
using System.Text;

namespace de.devcodemonkey.AIChecker.AIChecker
{
    internal class Program
    {
        private static string[] _args;
        static async Task Main(string[] args)
        {
            _args = args;
            // Set console encoding to UTF8 for status bar in Spectre.Console
            Console.OutputEncoding = Encoding.UTF8;
            // Setup DI
            var serviceProvider = await ConfigureServicesAsync();

            //await RunWithScopeAsync(serviceProvider, ["sendToLms", "-m", "Schreib mir ein Gedicht mit 100 Zeilen", "-s", "Du achtest darauf, dass sich alles reimt", "-r", "Requesttime check: | model: Phi-3.5-mini-instruct", "-c", "5", "-i", "5"]);
            //await RunWithScopeAsync(serviceProvider, ["sendToLms", "-r", "Test result set", "-m", "write me a poem over 10 lines"]);
            //await RunWithScopeAsync(serviceProvider, ["deleteResultSet", "-r", "cbc94e4a-868a-4751-aec1-9800dfbdcf08"]);
            //await RunWithScopeAsync(serviceProvider, ["viewResults", "-r", "7d26beed-3e04-4f7f-adb4-19bceca49503"]);
            //await RunWithScopeAsync(serviceProvider, ["viewProcessUsage"]);
            //await RunWithScopeAsync(serviceProvider, ["info"]);

            //await RunWithScopeAsync(serviceProvider, ["createMoreQuestions", "-r", "Create more questions | model xy", "-s", "Create a new question based on the answer"]);
            //await RunWithScopeAsync(serviceProvider, ["database", "-r"]);
            //await RunWithScopeAsync(serviceProvider, ["model", "-a"]);
            //await RunWithScopeAsync(serviceProvider, ["model", "-v"]);
            //await RunWithScopeAsync(serviceProvider, ["model", "-l"]);
            //await RunWithScopeAsync(serviceProvider, ["model", "-u"]);
            //await RunWithScopeAsync(serviceProvider, ["rankPrompt", "--help"]);

            //await RunWithScopeAsync(serviceProvider, ["recreateDatabase", "-f"]);
            //await RunWithScopeAsync(serviceProvider, ["importQuestions", "-p", "C:\\Users\\d-hoe\\source\\repos\\masterarbeit.wiki\\06_00_00-Ticketexport\\FAQs\\FAQ-Outlook.json","-c", "Outlook"]);            

            //await RunWithScopeAsync(serviceProvider, ["exportPromptRank", "-r", "Test result set", "-t", "Markdown"]);
            //await RunWithScopeAsync(serviceProvider, ["exportPromptRank", "-r", "Test result set", "-t", "Html"]);
            //await RunWithScopeAsync(serviceProvider, ["exportPromptRank", "-r", "Test result set", "-t", "Docx"]);
            //await RunWithScopeAsync(serviceProvider, ["exportPromptRank", "-r", "Test result set"]);

            //await RunWithScopeAsync(serviceProvider, ["exportPromptRank", "-r", "Prompt Bewertung: Bildbeschreibungen über ChatGpt erstellen"]);
            //await RunWithScopeAsync(serviceProvider, ["exportPromptRank", "-r", "Bild Beschreibungen über ChatGpt erstellen", "-t", "Markdown"]);
            //await RunWithScopeAsync(serviceProvider, ["exportPromptRank", "-r", "Bild Beschreibungen über ChatGpt erstellen", "-t", "Docx"]);

            //await RunWithScopeAsync(serviceProvider, ["deleteResultSet", "-r", "Rank"]);

            //await RunWithScopeAsync(serviceProvider, ["rankPrompt", "-r", "ChatGpt Test", "-p", "Prüft ob die Ausgabe funktioniert", "-m", "gpt-4o-mini-2024-07-18"]);
            //await RunWithScopeAsync(serviceProvider, ["rankPrompt", "-r", "ChatGpt Test", "-p", "Prüft ob die Ausgabe funktioniert", "-m", "bartowski/Llama-3.2-1B-Instruct-GGUF"]);
            //await RunWithScopeAsync(serviceProvider, ["exportPromptRank", "-r", "ChatGpt Test", "-t", "pdf"]);

            await RunWithScopeAsync(serviceProvider, args);
        }

        // The extracted method that runs the application with a scoped service provider.
        private static async Task RunWithScopeAsync(IServiceProvider serviceProvider, string[] args)
        {
            // Manually create a scope for scoped services
            using (var scope = serviceProvider.CreateScope())
            {
                var scopedProvider = scope.ServiceProvider;

                // Run the application with the provided args
                var app = scopedProvider.GetRequiredService<Application>();
                await app.RunAsync(args);
            }
        }

        private static async Task<IServiceProvider> ConfigureServicesAsync()
        {
            var services = new ServiceCollection();
            await AnsiConsole.Status().StartAsync("Loading app services...", async ctx =>
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                    .Build();

                services.AddSingleton<IConfiguration>(configuration);

                services.AddDbContext<AicheckerContext>(options =>
                {
                    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
                });

                // Create the database
                if (runMigration())
                {
                    // services.AddSingleton<MigrationService>();
                    using (var scope = services.BuildServiceProvider().CreateScope())
                    {
                        var services = scope.ServiceProvider;
                        MigrationService.RunMigrationIfNeeded(services);
                    }

                }
                services.AddScoped<IDefaultMethodesRepository, DefaultMethodesRepository>();

                MdServiceRegistrationExtensions.AddServiceAndDependencies(services);

                // Register services
                services.AddScoped<Application>();
                // Register plugins
                services.AddSingleton<IDeserializer<QuestionAnswer>, Deserializer<QuestionAnswer>>();
                services.AddSingleton<IAPIRequester, APIRequester>();
                services.AddSingleton<ISystemMonitor, SystemMonitor>();
                services.AddSingleton<IWslDatabaseService, WslDatabaseService>();
                services.AddSingleton<ILoadUnloadLms, LoadUnloadLms>();
                // Register use cases
                services.AddSingleton<IRecreateDatabaseUseCase, RecreateDatabaseUseCase>();
                services.AddSingleton<IImportQuestionAnswerUseCase, ImportQuestionAnswerUseCase>();
                services.AddSingleton<IDeleteAllQuestionAnswerUseCase, DeleteAllQuestionAnswerUseCase>();
                services.AddSingleton<IDeleteResultSetUseCase, DeleteResultSetUseCase>();
                services.AddSingleton<ICreateMoreQuestionsUseCase, CreateMoreQuestionsUseCase>();
                services.AddSingleton<IViewAverageTimeOfResultSetUseCase, ViewAverageTimeOfResultSetUseCase>();
                services.AddSingleton<IViewResultSetsUseCase, ViewResultSetsUseCase>();
                services.AddSingleton<IViewResultsOfResultSetUseCase, ViewResultsOfResultSetUseCase>();
                services.AddSingleton<ISendAPIRequestToLmStudioAndSaveToDbUseCase, SendAPIRequestToLmStudioAndSaveToDbUseCase>();
                services.AddSingleton<IViewGpuUsageUseCase, ViewGpuUsageUseCase>();
                services.AddSingleton<IStartStopDatabaseUseCase, StartStopDatabaseUseCase>();
                services.AddSingleton<IAddModelUseCase, AddModelUseCase>();
                services.AddSingleton<IViewModels, ViewModels>();
                services.AddSingleton<ILoadModelUseCase, LoadModelUseCase>();
                services.AddSingleton<IUnloadModelUseCase, UnloadModelUseCase>();
                services.AddSingleton<ICreatePromptRatingUseCase, CreatePromptRatingUseCase>();

                services.AddScoped<IExportPromptRatingUseCase, ExportPromptRatingUseCase>();

                services.AddSingleton<IBackupDatabaseUseCase, BackupDatabaseUseCase>(provider =>
                {
                    var wslDatabaseService = provider.GetRequiredService<IWslDatabaseService>();
                    var configuration = provider.GetRequiredService<IConfiguration>();

                    // Retrieve the Git settings from configuration
                    string gitRemoteUrl = configuration["Git:GitRemoteUrl"];
                    string gitRepositoryName = configuration["Git:GitRemoteName"];

                    // Create an instance of BackupDatabaseUseCase with the required parameters
                    return new BackupDatabaseUseCase(wslDatabaseService, gitRemoteUrl, gitRepositoryName);
                });
            });

            return services.BuildServiceProvider();
        }

        private static bool runMigration()
        {
            if (_args.Length == 1 && (
                _args.Contains("help") || _args.Contains("-h") || _args.Contains("--help")
            || _args.Contains("version") || _args.Contains("-v") || _args.Contains("--version")
            || _args.Contains("info") || _args.Contains("--info"))
            || _args.Contains("viewProcessUsage") || _args.Contains("--viewProcessUsage"))
                return false;
            return true;
        }
    }
}
