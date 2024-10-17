using de.devcodemonkey.AIChecker.UseCases.Interfaces;
using de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;
using System.Diagnostics;

namespace de.devcodemonkey.AIChecker.UseCases
{
    public enum DataExportType
    {
        Pdf,
        Markdown,
        Html
    }

    public class ExportPromptRatingUseCase : IExportPromptRatingUseCase
    {
        private readonly IExportPromptRating _exportPromptRating;
        private readonly IMdFile _mdFile;
        private readonly IMdFontStyles _mdFontStyles;
        private readonly IDefaultMethodesRepository _defaultMethodesRepository;


        public ExportPromptRatingUseCase(IExportPromptRating exportPromptRating, IMdFile mdFile, IDefaultMethodesRepository defaultMethodesRepository, IMdFontStyles mdFontStyles)
        {
            _exportPromptRating = exportPromptRating;
            _mdFile = mdFile;
            _mdFontStyles = mdFontStyles;
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

            var tableTestdata = _exportPromptRating.GetTableTestdata(DateTime.Now.ToLongDateString(), firstResult!.RequestCreated.ToShortDateString(), results.Count().ToString(), firstResult.MaxTokens.ToString(), firstResult.Temperature.ToString());
            _mdFile.Text.AppendLine(tableTestdata);

            // set rating
            _mdFontStyles.AddH3Text("Auswertung");

            var orderedResults = results
                .OrderBy(r => r.PromptRatingRound.Round)
                .ThenBy(r => r.Model.Value);


            // loop rounds
            for (var i = 1; i < orderedResults.Count(); i++)
            {
                var round = orderedResults.Where(r => r.PromptRatingRound.Round == i).ToList();

                var tableRound = _exportPromptRating.GetRunTable(
                    runNumber: i,
                    promptAnforderungen: round.FirstOrDefault().ResultSet.PromptRequierements.ToString(),
                    prompt: round.FirstOrDefault().Asked,
                    systemPrompt: round.FirstOrDefault().SystemPrompt.Value,
                    modelRatings: round.Select(r => (r.Model.Value, r.PromptRatingRound.Rating)).ToList()
                    );
                _mdFile.Text.AppendLine(tableRound);
            }

            // loop models
            _mdFontStyles.AddH3Text("Modell Informationen");

            var models = orderedResults.Where(r => r.PromptRatingRound.Round == 1)
                .Select(m => m.Model).ToList();

            for (var i = 0; i < models.Count(); i++)
            {
                var model = models[i];

                var tableModel = _exportPromptRating.GetModelDetailsTable(
                    modelNumber: i + 1,
                    modelName: model.Value,
                    baseModel: model.BaseModels,
                    modelDescriptionLink: model.Link,
                    modelSize: model.Size.ToString()
                    );
                _mdFile.Text.AppendLine(tableModel);
            }

            // export file
            var exportPath = Path.Combine(Path.GetTempPath(), "AiExports");
            if (!Directory.Exists(exportPath))
                Directory.CreateDirectory(exportPath);
            var fileName = DateTime.Now.ToString("yyyyMMdd_HHmmss");

            switch (dataExportType)
            {
                case DataExportType.Markdown:
                    _mdFile.ExportAsMarkdown(Path.Combine(exportPath, $"{fileName}.md"));
                    break;
                case DataExportType.Html:
                    _mdFile.ExportAsHtml(Path.Combine(exportPath, $"{fileName}.html"));
                    break;
                default:
                    await _mdFile.ExportToPdfAsync(Path.Combine(exportPath, $"{fileName}.pdf"));
                    break;
            }

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
