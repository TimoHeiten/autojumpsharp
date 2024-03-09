using System.Diagnostics;

namespace autojump.Input;

/// <summary>
/// Configuration for the application
/// </summary>
public static class Config
{
    /// <summary>
    /// read the configuration from config.json in the current directory
    /// </summary>
    /// <returns></returns>
    public static Configuration ReadConfig()
    {
        var stream = File.OpenRead("config.json");
        var configuration = System.Text.Json.JsonSerializer.Deserialize<Configuration>(stream)!;

        var value= Environment.GetEnvironmentVariable("AUTOJUMP_DB_PATH");
        if (value is not null)
        {
            configuration = configuration with { ConnectionPath = value };
        }

        return configuration!;
    }
}

[DebuggerDisplay("ConnectionPath: {ConnectionPath}, LogsEnabled: {LogsEnabled}")]
public sealed record Configuration(string ConnectionPath, bool LogsEnabled = false);