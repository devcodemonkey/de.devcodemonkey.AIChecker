using de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;
using ScottPlot;
using ScottPlot.PlotStyles;

namespace de.devcodemonkey.AIChecker.MarkdownExporter
{
    public class MdCharts : IMdCharts
    {
        private const string MdImageString = "![{0}]({1})";

        private readonly IMdFile _mdFile;

        public MdCharts()
        {

        }

        public MdCharts(IMdFile mdFile)
        {
            _mdFile = mdFile;
        }

        public string CreateBarChart(string path, double[] values, string[] descriptions, int width = 800, int height = 600)
        {
            if (values == null || values.Length == 0)
                throw new ArgumentException("Values are required.", nameof(values));
            if (descriptions == null || descriptions.Length == 0)
                throw new ArgumentException("Descriptions are required.", nameof(descriptions));
            if (values.Length != descriptions.Length)
                throw new ArgumentException("Values and descriptions must have the same length.");

            ScottPlot.Plot plot = new();

            plot.Axes.Margins(bottom: 0);
            plot.Axes.Bottom.TickLabelStyle.FontSize = 20;
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


            if (!Path.Exists(path))
                Directory.CreateDirectory(path);

            var filePath = Path.Combine(path, $"{DateTime.Now.ToString("yyyyMMddHHmmss")}.png");

            plot.SavePng(filePath, width, height);

            return filePath;
        }

        public void CreateBarChartAndAddToMd(string path, string urlPath, double[] values, string[] descriptions, string title, int width = 800, int height = 600)
        {
            var filePath = CreateBarChart(path, values, descriptions, width, height);
            CreateImageLink(path, values, descriptions, title, width, height, filePath);
        }

        public string CreatePieChart(string path, double[] values, string[] descriptions, int width = 800, int height = 600)
        {
            if (values == null || values.Length == 0)
                throw new ArgumentException("Values are required.", nameof(values));
            if (descriptions == null || descriptions.Length == 0)
                throw new ArgumentException("Descriptions are required.", nameof(descriptions));
            if (values.Length != descriptions.Length)
                throw new ArgumentException("Values and descriptions must have the same length.");

            ScottPlot.Plot plot = new();

            // Add a legend to the plot
            plot.ShowLegend();

            plot.Legend.FontSize = 20;

            var pieList = new List<PieSlice>();

            for (int i = 0; i < values.Length; i++)
            {
                pieList.Add(new PieSlice
                {
                    Value = values[i],
                    Label = $"{values[i] / values.Sum():P1}",
                    LabelFontSize = 20,
                    LabelBold = true,
                    FillColor = Configuration.ChartColors[i],
                    LegendText = $"{descriptions[i]} ({values[i] / values.Sum():P1})"                    
                });
            }

            var pie = plot.Add.Pie(pieList.ToArray());

            pie.SliceLabelDistance = 0.5;

            // Set the donut fraction
            //pie.DonutFraction = 0.5;

            // framless mode        
            plot.HideAxesAndGrid();

            if (!Path.Exists(path))
                Directory.CreateDirectory(path);

            var filePath = Path.Combine(path, $"{DateTime.Now.ToString("yyyyMMddHHmmss")}.png");

            plot.SavePng(filePath, width, height);

            return filePath;
        }

        public void CreatePieChartAndAddToMd(string path, string urlPath, double[] values, string[] descriptions, string title, int width = 800, int height = 600)
        {
            var filePath = CreatePieChart(path, values, descriptions, width, height);
            CreateImageLink(path, values, descriptions, title, width, height, filePath);
        }

        private void CreateImageLink(string path, double[] values, string[] descriptions, string title, int width, int height, string filePath)
        {
            // Read the image from file and convert to base64
            string base64Image;
            using (var image = File.OpenRead(filePath))
            {
                byte[] imageBytes = new byte[image.Length];
                image.Read(imageBytes, 0, imageBytes.Length);
                base64Image = Convert.ToBase64String(imageBytes);
            }

            //  Create the markdown image string            
            var base64MdImage = $"![{title}](data:image/png;base64,{base64Image})";

            _mdFile.Text.AppendLine(base64MdImage + "\n");
        }
    }
}
