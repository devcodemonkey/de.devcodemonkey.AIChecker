using de.devcodemonkey.AIChecker.CoreBusiness.Models;
using de.devcodemonkey.AIChecker.DataSource.APIRequester;
using de.devcodemonkey.AIChecker.DataSource.SystemMonitor;
using de.devcodemonkey.AIChecker.DataStore.PostgreSqlEF;
using de.devcodemonkey.AIChecker.Importer.JsonDeserializer;
using de.devcodemonkey.AIChecker.UseCases.Interfaces;
using de.devcodemonkey.AIChecker.UseCases;
using de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;
using LmsWrapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace de.devcodemonkey.AIChecker.AIChecker.Extenions;

public static class ServiceExtensions
{
    public static IServiceCollection ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AicheckerContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        });
        services.AddScoped<IDefaultMethodesRepository, DefaultMethodesRepository>();
        return services;
    }


    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {        
        services.AddScoped<IDeserializer<QuestionAnswer>, Deserializer<QuestionAnswer>>();
        services.AddScoped<IAPIRequester, APIRequester>();
        services.AddScoped<ISystemMonitor, SystemMonitor>();
        services.AddScoped<IWslDatabaseService, WslDatabaseService>();
        services.AddScoped<ILoadUnloadLms, LoadUnloadLms>();
        return services;
    }    

    public static IServiceCollection ConfigureDatabaseUseCases(this IServiceCollection services)
    {
        services.AddScoped<IBackupDatabaseUseCase, BackupDatabaseUseCase>(provider =>
        {
            var wslDatabaseService = provider.GetRequiredService<IWslDatabaseService>();
            var configuration = provider.GetRequiredService<IConfiguration>();

            // Retrieve the Git settings from configuration
            string gitRemoteUrl = configuration["Git:GitRemoteUrl"];
            string gitRepositoryName = configuration["Git:GitRemoteName"];

            // Create an instance of BackupDatabaseUseCase with the required parameters
            return new BackupDatabaseUseCase(wslDatabaseService, gitRemoteUrl, gitRepositoryName);
        });
        services.AddScoped<IRestoreDatabaseUseCase, RestoreDatabaseUseCase>(provider =>
        {
            var wslDatabaseService = provider.GetRequiredService<IWslDatabaseService>();
            var configuration = provider.GetRequiredService<IConfiguration>();

            // Retrieve the Git settings from configuration
            string gitRemoteUrl = configuration["Git:GitRemoteUrl"];
            string gitRepositoryName = configuration["Git:GitRemoteName"];

            // Create an instance of BackupDatabaseUseCase with the required parameters
            return new RestoreDatabaseUseCase(wslDatabaseService, gitRemoteUrl, gitRepositoryName);
        });
        return services;
    }

    public static IServiceCollection ConfigureUseCases(this IServiceCollection services)
    {
        // Register use cases
        services.AddScoped<IRecreateDatabaseUseCase, RecreateDatabaseUseCase>();
        services.AddScoped<IImportQuestionAnswerUseCase, ImportQuestionAnswerUseCase>();
        services.AddScoped<IDeleteAllQuestionAnswerUseCase, DeleteAllQuestionAnswerUseCase>();
        services.AddScoped<IDeleteResultSetUseCase, DeleteResultSetUseCase>();
        services.AddScoped<ICreateMoreQuestionsUseCase, CreateMoreQuestionsUseCase>();
        services.AddScoped<IViewAverageTimeOfResultSetUseCase, ViewAverageTimeOfResultSetUseCase>();
        services.AddScoped<IViewResultSetsUseCase, ViewResultSetsUseCase>();
        services.AddScoped<IViewResultsOfResultSetUseCase, ViewResultsOfResultSetUseCase>();
        services.AddScoped<ISendAndSaveApiRequestUseCase, SendAPIRequestAndSaveToDbUseCase>();
        services.AddScoped<IViewGpuUsageUseCase, ViewGpuUsageUseCase>();
        services.AddScoped<IStartStopDatabaseUseCase, StartStopDatabaseUseCase>();
        services.AddScoped<IAddModelUseCase, AddModelUseCase>();
        services.AddScoped<IViewModels, ViewModels>();
        services.AddScoped<ILoadModelUseCase, LoadModelUseCase>();
        services.AddScoped<IUnloadModelUseCase, UnloadModelUseCase>();
        services.AddScoped<ICreatePromptRatingUseCase, CreatePromptRatingUseCase>();
        services.AddScoped<IImportQuestionsFromResultsUseCase, ImportQuestionsFromResultsUseCase>();
        services.AddScoped<ISendQuestionsToLmsUseCase, SendQuestionsToLmsUseCase>();
        services.AddScoped<ICheckJsonFormatOfResultsUseCase, CheckJsonFormatOfResultsUseCase>();
        services.AddScoped<IExportPromptRatingUseCase, ExportPromptRatingUseCase>();
        return services;
    }
}
