namespace autojump;

/// <summary>
/// Callback holder for easier unit testing and mocking
/// </summary>
public sealed class Lookups // could also be named context or similar things
{
    public Action<string> Log { get; set; }
    public Func<string> GetUserDir { get; set; }
    public Configuration Configuration { get; set; } = new("");
    public Func<IEnumerable<string>, Lookups, string?> StoreLookup { get; set; }
    // ReSharper disable once InconsistentNaming
    public Func<DateTime, double, double> LastAccessedAndCurrentCount_ToNewCount { get; set; } = (lastAccessed, currentCount) => currentCount + 1;

    public Lookups(Func<string> getUserDir, Func<IEnumerable<string>, Lookups, string?> storeLookup, Action<string>? log = null!)
    {
        GetUserDir = getUserDir;
        StoreLookup = storeLookup;
        Log = log ?? Console.WriteLine;
    }
    
    public static Lookups Default => new(() 
        => Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), (_, _) => null);
}