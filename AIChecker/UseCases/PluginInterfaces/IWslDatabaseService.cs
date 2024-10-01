namespace de.devcodemonkey.AIChecker.DataStore.PostgreSqlEF
{
    public interface IWslDatabaseService
    {
        bool RunCommandInPowershell(string command);
        bool RunCommandOnWsl(string command);
        bool StartDatabase(bool runInBackground = false);
        bool StopDatabase();
    }
}