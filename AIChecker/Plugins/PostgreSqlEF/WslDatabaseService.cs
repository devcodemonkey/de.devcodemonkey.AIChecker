using System.Diagnostics;
using System.Runtime.Versioning;

namespace de.devcodemonkey.AIChecker.DataStore.PostgreSqlEF;

    [SupportedOSPlatform("windows")]
public class WslDatabaseService : IWslDatabaseService
{
    public bool StartDatabase(bool runInBackground = false)
    {
        string addD = string.Empty;
        if (runInBackground)
            addD = " -d";

        string command = "wsl -- bash -c 'mkdir -p /tmp/AiChecker && " +
                 "cd /tmp/AiChecker && " +
                 "rm -f docker-compose.yml && " +
                 "curl -L -o docker-compose.yml https://raw.github.com/devcodemonkey/de.devcodemonkey.AIChecker/main/AIChecker/docker/PostgreSQL/docker-compose.yml && " +
                 $"docker-compose up{addD}'";

        return RunCommandInPowershell(command);
    }

    public bool StopDatabase()
    {
        if (!RunCommandOnWsl("cd /tmp/AiChecker && docker-compose down"))
            return false;
        return true;
    }

    public bool BackupDatabaseToGit(string gitRemoteUrl, string gitRepositoryName)
    {
        if (string.IsNullOrEmpty(gitRemoteUrl))
            throw new ArgumentNullException("remote url must be set");
        if (string.IsNullOrEmpty(gitRepositoryName))
            throw new ArgumentNullException("repository name must be set");

        string backupName = $"{DateTime.Now:yyyyMMdd_HHmmss}_AiCheckerDB_backup";
        string gitBranchName = $"backup/{backupName}";
        string repoPath = $"/tmp/{gitRepositoryName}";

        // Step 1: Clone the Git repository
        if (!RunCommandOnWsl($"cd /tmp && rm {gitRepositoryName} -r -f && git clone {gitRemoteUrl}/{gitRepositoryName}.git"))
            return false;

        // Step 2: Create a new branch for the backup
        if (!RunCommandOnWsl($"cd {repoPath} && git checkout -b {gitBranchName}"))
            return false;

        // Step 3: Execute the database backup using Docker and store the output in a .sql file
        string backupFilePath = $"/tmp/{gitRepositoryName}/{backupName}.sql";
        string dockerCommand = $"docker exec -it aichecker-db-1 pg_dump -U AiChecker -h localhost -p 5432 AiCheckerDB > {backupFilePath}";
        if (!RunCommandOnWsl(dockerCommand))
            return false;

        // Step 4: Add the backup file to Git
        if (!RunCommandOnWsl($"cd {repoPath} && git add {backupName}.sql"))
            return false;

        // Step 5: Commit the backup file with a message
        if (!RunCommandOnWsl($"cd {repoPath} && git commit -m \"database backup {backupName}\""))
            return false;

        // Step 6: Push the new branch to the remote repository
        if (!RunCommandOnWsl($"cd {repoPath} && git push origin {gitBranchName}"))
            return false;

        // Step 7: Clean up the temporary files
        if (!RunCommandOnWsl($"cd /tmp && rm -rf {gitRepositoryName}"))
            return false;

        return true;
    }


    public bool RunCommandInPowershell(string command)
    {
        string powershellCommand = $"-NoExit -Command \"{command}\"";

        ProcessStartInfo processInfo = new ProcessStartInfo
        {
            FileName = "powershell.exe",
            Arguments = powershellCommand,
            UseShellExecute = true,
            CreateNoWindow = false
        };

        try
        {
            Process.Start(processInfo);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return false;
        }
    }

    public bool RunCommandOnWsl(string command)
    {
        ProcessStartInfo processInfo = new ProcessStartInfo
        {
            FileName = "wsl",
            Arguments = $"-- {command}",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        try
        {
            using (Process process = Process.Start(processInfo))
            {
                process.OutputDataReceived += (sender, args) => Console.WriteLine(args.Data);
                process.ErrorDataReceived += (sender, args) => Console.WriteLine(args.Data);

                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();

                // Check if the process exited successfully
                if (process.ExitCode != 0)
                {
                    Console.WriteLine($"Command failed with exit code {process.ExitCode}");
                    return false;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
            return false;
        }
        return true;
    }
}

