namespace de.devcodemonkey.AIChecker.MarkdownExporter
{
    public interface IMdTable
    {
        void AddRow(params string[] values);
        void AddTable();
        string ToString();
    }
}