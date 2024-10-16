using de.devcodemonkey.AIChecker.CoreBusiness.Models;
using de.devcodemonkey.AIChecker.DataSource.APIRequester;
using de.devcodemonkey.AIChecker.DataSource.SystemMonitor;
using de.devcodemonkey.AIChecker.DataStore.PostgreSqlEF;
using de.devcodemonkey.AIChecker.DataStore.SQLServerEF;
using de.devcodemonkey.AIChecker.Importer.JsonDeserializer;
using de.devcodemonkey.AIChecker.MarkdownExporter;
using de.devcodemonkey.AIChecker.MarkdownExporter.Export;
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
            //args = ["sendToLms", "-m", "Schreib mir ein Gedicht mit 100 Zeilen", "-s", "Du achtest darauf, dass sich alles reimt", "-r", "Requesttime check: | model: Phi-3.5-mini-instruct", "-c", "5", "-i", "5"];
            // args = ["sendToLms", "-s", "", "-r" ,"Test result set", "-m", "write me a poem over 10 lines"];
            //args = ["sendToLms", "-r" ,"Test result set", "-m", "write me a poem over 10 lines"];
            //args = ["deleteResultSet", "-r", "cbc94e4a-868a-4751-aec1-9800dfbdcf08"];
            //args = ["viewResults", "-r", "7d26beed-3e04-4f7f-adb4-19bceca49503"];
            //args = ["viewProcessUsage"];
            //args = ["info"];            
            // args = ["importQuestions", "-p", "/home/david/masterarbeit.wiki/06_00_00-Ticketexport/FAQs/FAQ-Outlook.json"];
            // args = ["createMoreQuestions", "-r", "Create more questions | model xy", "-s", "Create a new questions based on the answer"];
            //args = ["database"];
            //args = ["database", "-r"];
            //args = ["database", "-s"];
            //args = ["model"];
            //args = ["model", "-a"];
            //args = ["model", "-v"];
            //args = ["model", "-l"];
            //args = ["model", "-u"];

            //args = ["rankPrompt", "--help"];


            //args = ["rankPrompt", "-r", "Test result set", "-p", "JSON format\nother things", "-m", "lmstudio-community/Phi-3.5-mini-instruct-GGUF/Phi-3.5-mini-instruct-Q4_K_M.gguf,TheBloke/SauerkrautLM-7B-HerO-GGUF/sauerkrautlm-7b-hero.Q4_K_M.gguf"];

            //args = ["exportPromptRank", "-r", "Test result set", "-t", "Markdown"];
            args = ["exportPromptRank", "-r", "Test result set"];
            //args = ["recreateDatabase", "-f"];

            _args = args;
            // Set console encoding to UTF8 for status bar in Spectre.Console
            Console.OutputEncoding = Encoding.UTF8;
            // Setup DI
            var serviceProvider = await ConfigureServicesAsync();

            // Run the application
            var app = serviceProvider.GetRequiredService<Application>();
            await app.RunAsync(args);
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
                services.AddSingleton<Application>();
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

                services.AddSingleton<IExportPromptRatingUseCase, ExportPromptRatingUseCase>();

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
