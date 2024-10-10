namespace de.devcodemonkey.AIChecker.DataStore.PostgreSqlEF
{
    public interface IWslDatabaseService
    {
        bool BackupDatabaseToGit(string gitRemoteUrl, string gitRepositoryName);
        bool RunCommandInPowershell(string command);
        bool RunCommandOnWsl(string command);
        bool StartDatabase(bool runInBackground = false);
        bool StopDatabase();
    }
}