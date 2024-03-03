using System.Diagnostics;

namespace autojump;

public static class Config
{
    public static Configuration ReadConfig()
    {
        var stream = File.OpenRead("config.json");
        var configuration = System.Text.Json.JsonSerializer.Deserialize<Configuration>(stream);

        return configuration!;
    }
}

[DebuggerDisplay("ConnectionPath: {ConnectionPath}, LogsEnabled: {LogsEnabled}")]
public sealed record Configuration(string ConnectionPath, bool LogsEnabled = false);