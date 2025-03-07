﻿using de.devcodemonkey.AIChecker.CoreBusiness.MarkDownExporterModels;
using de.devcodemonkey.AIChecker.UseCases.Interfaces;
using de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;
using System.Diagnostics;
using System.Text;

namespace de.devcodemonkey.AIChecker.UseCases
{
    public class ExportPromptRatingUseCase : IExportPromptRatingUseCase
    {
        private readonly IExportPromptRating _exportPromptRating;
        private readonly IMdFile _mdFile;
        private readonly IMdFontStyles _mdFontStyles;
        private readonly IMdCharts _mdCharts;
        private readonly IDefaultMethodesRepository _defaultMethodesRepository;


        public ExportPromptRatingUseCase(IExportPromptRating exportPromptRating, IMdFile mdFile, IMdCharts mdCharts,
            IDefaultMethodesRepository defaultMethodesRepository, IMdFontStyles mdFontStyles)
        {
            _exportPromptRating = exportPromptRating;
            _mdFile = mdFile;
            _mdFontStyles = mdFontStyles;
            _mdCharts = mdCharts;
            _defaultMethodesRepository = defaultMethodesRepository;
        }

        public async Task ExecuteAsync(string resultSet, DataExportType dataExportType = DataExportType.Pdf, bool openFolder = true)
        {
            _mdFontStyles.AddH2Text("Prompt Bewertung");

            // set test data
            Guid resultSetId = await _defaultMethodesRepository.GetResultSetIdByValueAsync(resultSet);
            var results = await _defaultMethodesRepository.ViewResultsOfResultSetAsync(resultSetId);

            if (results.Count() == 0)
                return;

            var firstResult = results.FirstOrDefault();

            var tableTestdata = _exportPromptRating.GetTableTestdata(
                DateTime.Now.ToString(),
                firstResult?.RequestCreated?.ToShortDateString() ?? string.Empty,
                resultSet,
                firstResult?.ResultSet.Description,
                results.Count().ToString(),
                firstResult?.MaxTokens?.ToString() ?? string.Empty,
                firstResult?.Temperature?.ToString() ?? string.Empty
            );
            _mdFile.Text.AppendLine(tableTestdata);

            // set rating
            _mdFontStyles.AddH3Text("Auswertung");

            var orderedResults = results
                .OrderBy(r => r.PromptRatingRound.Round)
                .ThenBy(r => r.Model.Value);

            var promptRating = orderedResults.Select(r => r?.PromptRatingRound?.Round).Distinct();

            // loop rounds
            List<double> values = new();
            List<string> descriptions = new();
            for (var i = 1; i <= promptRating.Count(); i++)
            {
                var round = orderedResults.Where(r => r.PromptRatingRound.Round == i).ToList();

                var tableRound = _exportPromptRating.GetRunTable(
                    runNumber: i,
                    promptAnforderungen: round.FirstOrDefault()?.ResultSet.PromptRequierements ?? string.Empty,
                    prompt: round.FirstOrDefault()?.Asked ?? string.Empty,
                    systemPrompt: round.FirstOrDefault()?.SystemPrompt?.Value ?? string.Empty,
                    modelRatings: round.Select(r => (r.Message ?? string.Empty,
                                                    r.Model.Value ?? string.Empty,
                                                    r.PromptRatingRound?.Rating ?? 0,
                                                    r.PromptRatingRound?.ReasenRating ?? string.Empty)
                                                    ).ToList()
                );
                _mdFile.Text.AppendLine(tableRound);

                double sumOfRatings = round.Select(r => r.PromptRatingRound.Rating).Sum();
                values.Add(sumOfRatings / round.Count);
                descriptions.Add($"{i}. Durchlauf");
            }

            var exportPath = Path.Combine(Path.GetTempPath(), "AiExports");
            var imagePath = Path.Combine(exportPath, "img");

            if (dataExportType != DataExportType.Docx)
                _mdCharts.CreateBarChartAndAddToMd(imagePath, "img", values.ToArray(), descriptions.ToArray(), "chart");
            else
                _mdCharts.CreateBarChartAndAddToMd(imagePath, "img", values.ToArray(), descriptions.ToArray(), "chart", 600, 400);

            // loop models
            _mdFontStyles.AddH3Text("Modell Informationen");

            var models = orderedResults.Where(r => r.PromptRatingRound.Round == 1)
                .Select(m => m.Model).ToList();

            for (var i = 0; i < models.Count(); i++)
            {
                var model = models[i];

                var tableModel = _exportPromptRating.GetModelDetailsTable(
                    modelNumber: i + 1,
                    modelName: model.Value ?? string.Empty,
                    baseModel: model.BaseModels ?? string.Empty,
                    modelDescriptionLink: model.Link ?? string.Empty,
                    modelSize: model.Size?.ToString() ?? string.Empty,
                    parameter: model.Parameter ?? string.Empty
                );
                _mdFile.Text.AppendLine(tableModel);
            }

            // export file
            var fileName = $"{DateTime.Now.ToString("yyyyMMdd_HHmmss")}_{resultSet.Replace(" ", "_").Replace(":", "_")}";

            await _mdFile.Export(Path.Combine(exportPath, fileName), dataExportType);

            if (openFolder)
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = exportPath,
                    UseShellExecute = true
                });
            }
        }
    }
}
