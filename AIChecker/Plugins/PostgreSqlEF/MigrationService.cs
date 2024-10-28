using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace de.devcodemonkey.AIChecker.DataStore.PostgreSqlEF;

public static class MigrationService
{
    private static readonly string VersionFilePath = Path.Combine(Path.GetTempPath(), Assembly.GetEntryAssembly()?.GetName().Name ?? "Unknown", "app_version.txt");

    public static void RunMigrationIfNeeded(IServiceProvider services)
    {
        // Get the current app version from the assembly
        var currentAppVersion = GetAppVersion();

        // Get the stored app version from the text file
        var storedAppVersion = GetStoredAppVersion();

        if (storedAppVersion != currentAppVersion)
        {
            using (var scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AicheckerContext>();
                try
                {
                    // Check if there are pending migrations
                    var pendingMigrations = context.Database.GetPendingMigrations();
                    if (pendingMigrations.Any() || storedAppVersion != currentAppVersion)
                    {
                        Console.WriteLine("Applying pending migrations...");
                        context.Database.Migrate();
                        Console.WriteLine("Migrations applied successfully.");

                        // Update the version in the text file after migration
                        UpdateStoredAppVersion(currentAppVersion);
                    }
                    else
                    {
                        Console.WriteLine("No pending migrations. Database is up to date.");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error: {e.Message}");
                    Console.WriteLine("Please check your connection string in appsettings.json or start the database.");
                }
            }
        }
    }

    private static string GetAppVersion()
    {
        // Get the entry assembly (main project) instead of the current assembly
        var entryAssembly = Assembly.GetEntryAssembly();

        // Get the version from the entry assembly
        var version = entryAssembly?
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion
            ?? entryAssembly?.GetName().Version?.ToString();

        return version ?? "Unknown";
    }

    private static string GetStoredAppVersion()
    {
        if (File.Exists(VersionFilePath))
        {
            return File.ReadAllText(VersionFilePath).Trim();
        }
        return string.Empty;
    }

    private static void UpdateStoredAppVersion(string version)
    {        
        var directoryPath = Path.GetDirectoryName(VersionFilePath);
        if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);

        File.WriteAllText(VersionFilePath, version);
    }
}
