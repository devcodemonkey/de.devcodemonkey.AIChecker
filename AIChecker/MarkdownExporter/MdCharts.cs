using de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;
using ScottPlot;

namespace de.devcodemonkey.AIChecker.MarkdownExporter
{
    public class MdCharts
    {
        private readonly IMdFile _mdFile;

        public MdCharts()
        {

        }

        public MdCharts(IMdFile mdFile)
        {
            _mdFile = mdFile;
        }

        public string CreateBarChart(double[] values, string[] descriptions, int width = 400, int heigth = 300)
        {
            if (values == null || values.Length == 0)
                throw new ArgumentException("Values are required.", nameof(values));
            if (descriptions == null || descriptions.Length == 0)
                throw new ArgumentException("Descriptions are required.", nameof(descriptions));
            if (values.Length != descriptions.Length)
                throw new ArgumentException("Values and descriptions must have the same length.");

            ScottPlot.Plot plot = new();

            var barList = new List<Bar>();

            for (int i = 0; i < values.Length; i++)
            {
                barList.Add(new Bar
                {
                    Position = i,
                    Value = values[i],
                    FillColor = Configuration.ChartColors[i],
                });
            }

            var bar = plot.Add.Bars(barList.ToArray());

            Tick[] ticks = descriptions.Select((description, index) => new Tick(index, description)).ToArray();

            plot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual(ticks);


            if (!Path.Exists(Configuration.TempPath))
                Directory.CreateDirectory(Configuration.TempPath);

            var filePath = Path.Combine(Configuration.TempPath, $"{DateTime.Now.ToString("yyyyMMddHHmmss")}.png");

            plot.SavePng(filePath, width, heigth);

            return filePath;
        }
    }
}
