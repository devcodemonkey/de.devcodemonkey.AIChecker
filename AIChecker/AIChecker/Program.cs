using de.devcodemonkey.AIChecker.CoreBusiness.Models;
using de.devcodemonkey.AIChecker.DataSource.APIRequester;
using de.devcodemonkey.AIChecker.DataSource.SystemMonitor;
using de.devcodemonkey.AIChecker.DataStore.SQLServerEF;
using de.devcodemonkey.AIChecker.Importer.JsonDeserializer;
using de.devcodemonkey.AIChecker.UseCases;
using de.devcodemonkey.AIChecker.UseCases.Interfaces;
using de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;
using System.Text;

namespace de.devcodemonkey.AIChecker.AIChecker
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // Set console encoding to UTF8 for stutus bar in Spectre.Console
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
                using (var scope = services.BuildServiceProvider().CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<AicheckerContext>();
                    try
                    {
                        context.Database.Migrate();
                    }
                    catch (System.Exception e)
                    {
                        AnsiConsole.MarkupLine($"[red]Error: {e.Message}[/]");

                        AnsiConsole.MarkupLine("[red]Please check your connection string in appsettings.json or start the database[/]");
                    }

                }

                services.AddScoped<IDefaultMethodesRepository, DefaultMethodesRepository>();
                // Register services
                services.AddSingleton<Application>();
                // Register plugins
                services.AddSingleton<IDeserializer<QuestionAnswer>, Deserializer<QuestionAnswer>>();
                services.AddSingleton<IAPIRequester, APIRequester>();
                services.AddSingleton<ISystemMonitor, SystemMonitor>();
                // Register use cases
                services.AddSingleton<IRecreateDatabaseUseCase, RecreateDatabaseUseCase>();
                services.AddSingleton<IImportQuestionAnswerUseCase, ImportQuestionAnswerUseCase>();
                services.AddSingleton<IDeleteAllQuestionAnswerUseCase, DeleteAllQuestionAnswerUseCase>();
                services.AddSingleton<IDeleteResultSetUseCase, DeleteResultSetUseCase>();
                services.AddSingleton<ICreateMoreQuestionsUseCase, CreateMoreQuestionsUseCase>();
                services.AddSingleton<IViewAvarageTimeOfResultSetUseCase, ViewAvarageTimeOfResultSetUseCase>();
                services.AddSingleton<IViewResultSetsUseCase, ViewResultSetsUseCase>();
                services.AddSingleton<IViewResultsOfResultSetUseCase, ViewResultsOfResultSetUseCase>();
                services.AddSingleton<ISendAPIRequestToLmStudioAndSaveToDbUseCase, SendAPIRequestToLmStudioAndSaveToDbUseCase>();
            });

            return services.BuildServiceProvider();
        }
    }
}
