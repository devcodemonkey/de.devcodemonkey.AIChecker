using System.Diagnostics;
using System.Runtime.Versioning;

namespace de.devcodemonkey.AIChecker.DataStore.PostgreSqlEF
{
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
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    process.WaitForExit();

                    Console.WriteLine($"Output {output}");

                    if (!string.IsNullOrEmpty(error))
                    {
                        Console.WriteLine($"Error: {error}");
                        if (!error.ToLower().Contains("container"))
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
}
