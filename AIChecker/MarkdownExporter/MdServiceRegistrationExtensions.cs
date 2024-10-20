using de.devcodemonkey.AIChecker.MarkdownExporter.Export;
using de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;
using Microsoft.Extensions.DependencyInjection;

namespace de.devcodemonkey.AIChecker.MarkdownExporter
{
    public static class MdServiceRegistrationExtensions
    {
        public static IServiceCollection AddServiceAndDependencies(this IServiceCollection services)
        {
            services.AddScoped<IMdFile, MdFile>();
            services.AddScoped<IMdFontStyles, MdFontStyles>();
            services.AddScoped<IExportPromptRating, ExportPromptRating>();            
            services.AddScoped<IMdTable, MdTable>();
            services.AddScoped<IMdCharts, MdCharts>();
            return services;
        }
    }
}
