using System.Diagnostics;
using autojump.Core;

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
        var configPath = Path.Combine(Context.Default.GetUserDir(), "autojump", "config.json");
        var stream = File.OpenRead(configPath);
        var configuration = System.Text.Json.JsonSerializer.Deserialize<Configuration>(stream)!;
        return configuration!;
    }
}

[DebuggerDisplay("LogsEnabled: {LogsEnabled}")]
public sealed record Configuration(bool LogsEnabled = false);