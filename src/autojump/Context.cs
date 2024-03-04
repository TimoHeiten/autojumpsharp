namespace autojump;

/// <summary>
/// Callback holder for easier unit testing and mocking
/// </summary>
public sealed class Context // could also be named context or similar things
{
    public Action<string> Log { get; set; }
    public Func<string> GetUserDir { get; set; }
    public Configuration Configuration { get; set; } = new("");
    // ReSharper disable once InconsistentNaming
    public Func<DateTime, double, double> LastAccessedAndCurrentCount_ToNewCount { get; set; } = (lastAccessed, currentCount) => currentCount + 1;

    public Context(Func<string> getUserDir, Action<string>? log = null!)
    {
        GetUserDir = getUserDir;
        Log = log ?? Console.WriteLine;
    }
    
    public static Context Default => new(() 
        => Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) );
}