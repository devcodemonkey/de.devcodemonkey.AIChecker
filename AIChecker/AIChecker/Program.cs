using de.devcodemonkey.AIChecker.CoreBusiness.Models;
using de.devcodemonkey.AIChecker.DataSource.APIRequester;
using de.devcodemonkey.AIChecker.DataStore.SQLServerEF;
using de.devcodemonkey.AIChecker.Importer.JsonDeserializer;
using de.devcodemonkey.AIChecker.UseCases;
using de.devcodemonkey.AIChecker.UseCases.Interfaces;
using de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;
using Microsoft.Extensions.DependencyInjection;

namespace de.devcodemonkey.AIChecker.AIChecker
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // Setup DI
            var serviceProvider = ConfigureServices();

            // Run the application
            var app = serviceProvider.GetRequiredService<Application>();
            await app.RunAsync(args);
        }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            // Register services
            services.AddSingleton<Application>();
            // Register plugins
            services.AddSingleton<IDeserializer<QuestionAnswer>, Deserializer<QuestionAnswer>>();
            services.AddSingleton<IDefaultMethodesRepository, DefaultMethodesRepository>();
            services.AddSingleton<IAPIRequester, APIRequester>();
            // Register use cases
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
