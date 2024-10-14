using System.Diagnostics;
using de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;

namespace LmsWrapper
{
    public class LoadUnloadLms : ILoadUnloadLms
    {
        public bool LoadLms(string modelName)
        {
            UnloadLms();
            return RunProcess("lms", $"load {modelName} --gpu max");
        }

        public bool UnloadLms()
        {
            return RunProcess("lms", $"unload --all");
        }

        private bool RunProcess(string command, string arguments)
        {
            try
            {
                // Configure the process start info
                ProcessStartInfo processInfo = new ProcessStartInfo
                {
                    FileName = command,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = false
                };

                // Start the process
                using (Process process = Process.Start(processInfo))
                {
                    // Capture standard output and error
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    // Wait for the process to finish
                    process.WaitForExit();

                    // Check for errors
                    if (process.ExitCode != 0)
                    {
                        // Log the error output if needed
                        Console.WriteLine($"Error: {error}");
                        return false;
                    }

                    // Optionally, log the successful output
                    Console.WriteLine($"Output: {output}");
                }

                return true; // Process ran successfully
            }
            catch (Exception ex)
            {
                // Log the exception details
                Console.WriteLine($"Exception: {ex.Message}");
                return false;
            }
        }
    }
}
