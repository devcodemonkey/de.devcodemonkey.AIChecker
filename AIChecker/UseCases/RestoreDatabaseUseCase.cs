using de.devcodemonkey.AIChecker.DataStore.PostgreSqlEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace de.devcodemonkey.AIChecker.UseCases
{
    public class RestoreDatabaseUseCase : IRestoreDatabaseUseCase
    {
        private readonly IWslDatabaseService _wslDatabaseService;
        private string _gitRemoteUrl;
        private string _gitRepositoryName;

        public RestoreDatabaseUseCase(IWslDatabaseService wslDatabaseService, string gitRemoteUrl, string gitRepositoryName)
        {
            _wslDatabaseService = wslDatabaseService;
            _gitRemoteUrl = gitRemoteUrl;
            _gitRepositoryName = gitRepositoryName;
        }

        public bool Execute(string gitBranchName)
        {
            return _wslDatabaseService.RestoreDatabaseFromGit(_gitRemoteUrl, _gitRepositoryName, gitBranchName);
        }
    }
}
