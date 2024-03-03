using autojump;

public static class Program
{
    public static void Main(string[] args)
    {
        // configuration and arguments
        var config = Config.ReadConfig();
        var argv = new Args(args);

        // setup context / lookups
        var lookups = Lookups.Default;
        lookups.Configuration = config;
        lookups.StoreLookup = SqliteStore.Lookup;
        lookups.Log = msg => 
        {
            if (config.LogsEnabled)
            {
                Console.WriteLine(msg);
            }
        };
        lookups.LastAccessedAndCurrentCount_ToNewCount = Bump;

        // run actual command and return result to sdout
        var command = SelectCommand(argv, lookups);
        var result = command.Invoke();

        if (result.Success)
        {
            Console.WriteLine(result.Value);
        }
    }

    public static Func<Result> SelectCommand(Args args, Lookups lookups)
    {
        return args.Name switch
        {
            Args.Names.CD => () =>
            {
                lookups.Log($"ChangeDirectoryCommand: {args.Name} {string.Join(" ", args)}");

                var dir = lookups.GetUserDir();
                var lookup = lookups.StoreLookup(args, lookups);

                return lookup is not null ? Result.Ok(lookup) : Result.Ok(dir);
            },
            Args.Names.Bump => () => throw new NotImplementedException(),

            _ => Result.Fail
        };
    }

    public static double Bump(DateTime lastAccessed, double currentCount)
    {
        var now = DateTime.Now;
        var diff = now - lastAccessed;
        if (diff < TimeSpan.FromHours(1))
        {
            return currentCount * 4;
        }
        if (diff < TimeSpan.FromDays(1))
        {
            return currentCount * 2;
        }
        if (diff < TimeSpan.FromDays(7))
        {
            return currentCount;
        }
        if (diff < TimeSpan.FromDays(30))
        {
            return currentCount * 0.5;
        }
        if (diff < TimeSpan.FromDays(365))
        {
            return currentCount * 0.25;
        }
        return currentCount / 8;
    }
}