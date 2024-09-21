using de.devcodemonkey.AIChecker.CoreBusiness.Models;
using de.devcodemonkey.AIChecker.DataSource.APIRequester;
using de.devcodemonkey.AIChecker.DataSource.SystemMonitor;
using de.devcodemonkey.AIChecker.DataStore.SQLServerEF;
using de.devcodemonkey.AIChecker.Importer.JsonDeserializer;
using de.devcodemonkey.AIChecker.UseCases;
using de.devcodemonkey.AIChecker.UseCases.Interfaces;
using de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
            var serviceProvider = ConfigureServices();

            // Run the application
            var app = serviceProvider.GetRequiredService<Application>();
            await app.RunAsync(args);
        }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddDbContext<AicheckerContext>();
            
            // Create the database
            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AicheckerContext>();
                context.Database.Migrate();
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

            return services.BuildServiceProvider();
        }
    }
}
