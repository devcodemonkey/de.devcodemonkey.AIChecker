using de.devcodemonkey.AIChecker.DataStore.PostgreSqlEF;
using de.devcodemonkey.AIChecker.UseCases.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace de.devcodemonkey.AIChecker.UseCases
{
    public class BackupDatabaseUseCase : IBackupDatabaseUseCase
    {
        private readonly IWslDatabaseService _wslDatabaseService;
        private string _gitRemoteUrl;
        private string _gitRepositoryName;

        public BackupDatabaseUseCase(IWslDatabaseService wslDatabaseService, string gitRemoteUrl, string gitRepositoryName)
        {
            _wslDatabaseService = wslDatabaseService;
            _gitRemoteUrl = gitRemoteUrl;
            _gitRepositoryName = gitRepositoryName;
        }

        public bool Execute()
        {
            return _wslDatabaseService.BackupDatabaseToGit(_gitRemoteUrl, _gitRepositoryName);
        }
    }
}
