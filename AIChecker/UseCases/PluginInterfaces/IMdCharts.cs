namespace de.devcodemonkey.AIChecker.UseCases.PluginInterfaces
{
    public interface IMdCharts
    {
        string CreateBarChart(string path, double[] values, string[] descriptions, int width = 400, int heigth = 300);        
        void CreateBarChartAndAddToMd(string path, string urlPath, double[] values, string[] descriptions, string title, int width = 400, int heigth = 300);
    }
}