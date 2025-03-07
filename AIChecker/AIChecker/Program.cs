﻿using de.devcodemonkey.AIChecker.AIChecker.Extenions;
using de.devcodemonkey.AIChecker.DataStore.PostgreSqlEF;
using de.devcodemonkey.AIChecker.MarkdownExporter;
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

            #region Test
            //await RunWithScopeAsync(serviceProvider, ["sendToLms", "-m", "Schreib mir ein Gedicht mit 100 Zeilen", "-s", "Du achtest darauf, dass sich alles reimt", "-r", "Requesttime check: | model: Phi-3.5-mini-instruct", "-c", "5", "-i", "5"]);
            //await RunWithScopeAsync(serviceProvider, ["sendToLms", "-r", "Test result set", "-m", "write me a poem over 10 lines"]);
            //await RunWithScopeAsync(serviceProvider, ["viewResults", "-r", "Test result set"]);
            //await RunWithScopeAsync(serviceProvider, ["deleteResultSet", "-r", "cbc94e4a-868a-4751-aec1-9800dfbdcf08"]);
            //await RunWithScopeAsync(serviceProvider, ["viewResults", "-r", "7d26beed-3e04-4f7f-adb4-19bceca49503"]);
            //await RunWithScopeAsync(serviceProvider, ["viewProcessUsage"]);
            //await RunWithScopeAsync(serviceProvider, ["info"]);

            //await RunWithScopeAsync(serviceProvider, ["createMoreQuestions", "-r", "Create more questions | model xy", "-s", "Create a new question based on the answer"]);
            //await RunWithScopeAsync(serviceProvider, ["database", "-r"]);
            //await RunWithScopeAsync(serviceProvider, ["model", "-a"]);
            //await RunWithScopeAsync(serviceProvider, ["model", "-v"]);
            //await RunWithScopeAsync(serviceProvider, ["model", "-l"]);
            //await RunWithScopeAsync(serviceProvider, ["model", "-l", "-m", "bartowski/Mistral-Small-Instruct-2409-GGUF"]);
            //await RunWithScopeAsync(serviceProvider, ["model", "-u"]);
            //await RunWithScopeAsync(serviceProvider, ["rankPrompt", "--help"]);

            //await RunWithScopeAsync(serviceProvider, ["recreateDatabase", "-f"]);
            //await RunWithScopeAsync(serviceProvider, ["importQuestions", "-p", "C:\\Users\\d-hoe\\source\\repos\\masterarbeit.wiki\\06_00_00-Ticketexport\\FAQs\\FAQ-Outlook.json","-c", "Outlook"]);            

            //await RunWithScopeAsync(serviceProvider, ["exportPromptRank", "-r", "Test result set", "-t", "Markdown"]);
            //await RunWithScopeAsync(serviceProvider, ["exportPromptRank", "-r", "Test result set", "-t", "Html"]);
            //await RunWithScopeAsync(serviceProvider, ["exportPromptRank", "-r", "Test result set", "-t", "Docx"]);
            //await RunWithScopeAsync(serviceProvider, ["exportPromptRank", "-r", "Test result set"]);

            //await RunWithScopeAsync(serviceProvider, ["exportPromptRank", "-r", "Prompt Bewertung: Bildbeschreibungen über ChatGpt erstellen (Nr. 1)"]);
            //await RunWithScopeAsync(serviceProvider, ["exportPromptRank", "-r", "Bild Beschreibungen über ChatGpt erstellen", "-t", "Markdown"]);
            //await RunWithScopeAsync(serviceProvider, ["exportPromptRank", "-r", "Bild Beschreibungen über ChatGpt erstellen", "-t", "Docx"]);

            //await RunWithScopeAsync(serviceProvider, ["deleteResultSet", "-r", "Rank"]);

            //await RunWithScopeAsync(serviceProvider, ["rankPrompt", "-r", "ChatGpt Test", "-p", "Prüft ob die Ausgabe funktioniert", "-m", "gpt-4o-mini-2024-07-18"]);

            //await RunWithScopeAsync(serviceProvider, ["deleteResultSet", "-r", "Test"]);
            //await RunWithScopeAsync(serviceProvider, ["rankPrompt", "-r", "Test", "-p", "Prüft ob die Ausgabe funktioniert", "-m", "bartowski/Llama-3.2-1B-Instruct-GGUF"]);

            //await RunWithScopeAsync(serviceProvider, ["exportPromptRank", "-r", "ChatGpt Test", "-t", "pdf"]);

            //            await RunWithScopeAsync(serviceProvider, ["deleteResultSet", "-r", "Prompt Ranking für Fragen erstellen (1. Versuch)"]);
            //await RunWithScopeAsync(serviceProvider, ["deleteResultSet", "-r", "Test"]);
            //await RunWithScopeAsync(serviceProvider, ["rankPrompt", "-r", "Test", "-p", @"* Ausgabe im vorgegebenen JSON-Format
            //* Ausgabe der reinen JSON-Ausgabe, ohne Text vor oder nach dem Beginn, damit validiert werden kann
            //* Inhaltlich korrekt abgeleitete Fragen aus der Antwort
            //* Unterschiedliche Fragen
            //* Erzeugung von 10 Fragen", "-m", "bartowski/Llama-3.2-1B-Instruct-GGUF"]);

            //await RunWithScopeAsync(serviceProvider, ["rankPrompt", "-r", "Test", "-p", @"* Ausgabe im vorgegebenen JSON-Format
            //* Ausgabe der reinen JSON-Ausgabe, ohne Text vor oder nach dem Beginn, damit validiert werden kann
            //* Inhaltlich korrekt abgeleitete Fragen aus der Antwort
            //* Unterschiedliche Fragen
            //* Erzeugung von 10 Fragen", "-m", "gpt-4o-mini-2024-07-18"]);

            // the system prompt or the the prompt must include json in the prompt
            //await RunWithScopeAsync(serviceProvider, ["rankPrompt", "-r", "Test", "-p", @"* Ausgabe im vorgegebenen JSON-Format
            //* Ausgabe der reinen JSON-Ausgabe, ohne Text vor oder nach dem Beginn, damit validiert werden kann
            //* Inhaltlich korrekt abgeleitete Fragen aus der Antwort
            //* Unterschiedliche Fragen
            //* Erzeugung von 10 Fragen", "-m", "gpt-4o-mini-2024-07-18", "-s", "{ \"type\": \"json_object\" }"]);

            //            await RunWithScopeAsync(serviceProvider, ["deleteResultSet", "-r", "Prompt Ranking für Fragen erstellen (Nr. 3)"]);

            //            var responseFormat = @"
            //{
            //    ""type"": ""json_schema"",
            //    ""json_schema"": {
            //      ""name"": ""questions"",
            //      ""schema"": {
            //        ""type"": ""object"",
            //        ""properties"": {
            //          ""questions"": {
            //            ""type"": ""array"",
            //            ""items"": {
            //              ""type"": ""object"",
            //              ""properties"": {
            //                ""Question"": {
            //                  ""type"": ""string"",
            //                  ""description"": ""The text of the question.""
            //                }
            //              },
            //              ""required"": [""Question""],
            //              ""additionalProperties"": false
            //            }
            //          }
            //        },
            //        ""required"": [""questions""],
            //        ""additionalProperties"": false
            //      },
            //      ""strict"": true
            //    }
            //  }
            //";

            //            var description = @"
            //Erstellt ein Ranking für einen Systemprompt für die Generierung von Fragen. Hierfür wird eine Frage aus dem Qutlook-FAQ verwendet.
            //Beim Test ""Prompt Ranking für Fragen erstellen (Nr. 2)"" konnte inhaltlich gute Ausgabe erzeugt werden. Allerdings waren nicht alle Ergebnisse im JSON Format
            //In diesem Test wird daher mit dem folgendem JSON-Schema gearbeitet:
            //{
            //    ""type"": ""json_schema"",
            //    ""json_schema"": {
            //      ""name"": ""questions"",
            //      ""schema"": {
            //        ""type"": ""object"",
            //        ""properties"": {
            //          ""questions"": {
            //            ""type"": ""array"",
            //            ""items"": {
            //              ""type"": ""object"",
            //              ""properties"": {
            //                ""Question"": {
            //                  ""type"": ""string"",
            //                  ""description"": ""The text of the question.""
            //                }
            //              },
            //              ""required"": [""Question""],
            //              ""additionalProperties"": false
            //            }
            //          }
            //        },
            //        ""required"": [""questions""],
            //        ""additionalProperties"": false
            //      },
            //      ""strict"": true
            //    }
            //}
            //Das Einbinden eines Dateiformats wird auf der folgenden Seite https://platform.openai.com/docs/guides/structured-outputs?context=ex1&lang=curl#introduction beschrieben.
            //";
            //            await RunWithScopeAsync(serviceProvider, ["rankPrompt", "-r", "Prompt Ranking für Fragen erstellen (Nr. 3)", "-p", description, "-m", "gpt-4o-mini-2024-07-18", "-s", responseFormat]);

            //await RunWithScopeAsync(serviceProvider, ["exportPromptRank", "-r", "Prompt Ranking für Fragen erstellen (Nr. 1)", "-t", "pdf"]);


            //await RunWithScopeAsync(serviceProvider, ["deleteResultSet", "-r", "ChatGpt Test mini model"]);

            //await RunWithScopeAsync(serviceProvider, ["sendToLms", "-r", "ChatGpt Test mini model",
            //    "-t", "21", "-u", "-i", "10", "-m", "Write me a poem", "--model", "gpt-4o-mini-2024-07-18", "-s", "You are helpful",
            //    "-c", "4"]);


            //await RunWithScopeAsync(serviceProvider, ["deleteResultSet", "-r", "Fragezuordnung für Testverfahren Skala Outlook allgemein (Nr. 1) Test"]);
            //await RunWithScopeAsync(serviceProvider, ["sendToLms", "-r", "Fragezuordnung für Testverfahren Skala Outlook allgemein (Nr. 1) Test", "--questionCategory", "Outlook created Questions over gpt-4o-mini,Teams Citrix created Questions over gpt-4o-mini,Teams allgemein created Questions over gpt-4o-mini,Azubi FAQ created Questions over gpt-4o-mini", "--questionsCorrect", "-s", "", "-u", "1", "-w"]);
            //await RunWithScopeAsync(serviceProvider, ["checkJson", "-r", "Fragezuordnung für Testverfahren Skala (Nr. 2) Teams Citrix korrekt geprüfte Fragen Test"]);

            #endregion

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

                services
                    .ConfigureDatabase(configuration)
                    .ConfigureServices()
                    .ConfigureUseCases()
                    .ConfigureDatabaseUseCases();

                // Create the database
                if (runMigration())
                    using (var scope = services.BuildServiceProvider().CreateScope())
                    {
                        var services = scope.ServiceProvider;
                        MigrationService.RunMigrationIfNeeded(services);
                    }

                MdServiceRegistrationExtensions.AddServiceAndDependencies(services);

                // Register services
                services.AddScoped<Application>();
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
