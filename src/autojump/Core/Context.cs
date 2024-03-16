using System;
using autojump.Input;

namespace autojump.Core;

/// <summary>
/// Callback holder for easier unit testing and mocking
/// </summary>
public sealed class Context // could also be named context or similar things
{
    /// <summary>
    /// The logging callback
    /// </summary>
    public Action<string> Log { get; set; }
    /// <summary>
    /// A callback to get the user directory, duh
    /// </summary>
    public Func<string> GetUserDir { get; set; }
    /// <summary>
    /// The parsed configuration 
    /// </summary>
    public Configuration Configuration { get; set; } = new();
    // ReSharper disable once InconsistentNaming
    /// <summary>
    /// Essentially a callback that indirects the bump alogrithm.
    /// </summary>
    public Func<DateTime, double, double> LastAccessedAndCurrentCount_ToNewCount { get; set; } = (lastAccessed, currentCount) => currentCount + 1;

    private Context(Func<string> getUserDir, Action<string>? log = null!)
    {
        GetUserDir = getUserDir;
        Log = log ?? Console.WriteLine;
    }

    public static Context Default => new(() 
        => Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));

    public static Context Create(Configuration config, Func<DateTime, double, double> bump)
    {
        // setup context / lookups
        var context = Context.Default;
        context.Configuration = config;
        context.Log = msg => 
        {
            if (config.LogsEnabled)
            {
                Console.WriteLine(msg);
            }
        };
        context.LastAccessedAndCurrentCount_ToNewCount = bump;
        return context;
    }
}