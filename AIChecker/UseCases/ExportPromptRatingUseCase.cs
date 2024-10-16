using de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace de.devcodemonkey.AIChecker.UseCases
{
    public enum DataExportType
    {
        Pdf,
        Markdown
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

        public async Task ExecuteAsync(DataExportType dataExportType, string resultSet)
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
            for (var i = 1; i < orderedResults.Count() - 1; i++)
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

            for (var i = 1; i < models.Count() - 1; i++)
            {
                var model = models[i];

                var tableModel = _exportPromptRating.GetModelDetailsTable(
                    modelNumber: i,
                    modelName: model.Value,
                    baseModel: model.BaseModels,
                    modelDescriptionLink: model.Link,
                    modelSize: model.Size.ToString()
                    );
                _mdFile.Text.AppendLine(tableModel);
            }

            // export file
            await _mdFile.ExportToPdfAsync(Path.Combine("c:\\temp", "PromptRating.pdf"));
        }
    }
}
