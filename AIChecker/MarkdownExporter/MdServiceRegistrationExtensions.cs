using de.devcodemonkey.AIChecker.MarkdownExporter.Export;
using de.devcodemonkey.AIChecker.UseCases;
using de.devcodemonkey.AIChecker.UseCases.Interfaces;
using de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace de.devcodemonkey.AIChecker.MarkdownExporter
{
    public static class MdServiceRegistrationExtensions
    {
        public static IServiceCollection AddServiceAndDependencies(this IServiceCollection services)
        {
            services.AddScoped<IMdFile, MdFile>();
            services.AddScoped<IMdFontStyles, MdFontStyles>();
            services.AddScoped<IExportPromptRating, ExportPromptRating>();            
            return services;
        }
    }
}
