namespace de.devcodemonkey.AIChecker.UseCases.PluginInterfaces
{
    public interface IMdTable
    {
        void AddRow(params string[] values);
        void AddTable();
        string ToString();
    }
}