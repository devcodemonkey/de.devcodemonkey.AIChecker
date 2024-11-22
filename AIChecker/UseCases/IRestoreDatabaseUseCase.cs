namespace de.devcodemonkey.AIChecker.UseCases
{
    public interface IRestoreDatabaseUseCase
    {        
        bool Execute(string gitBranchName);
    }
}