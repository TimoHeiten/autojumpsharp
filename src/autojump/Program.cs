using autojump;
using autojump.Store;

public static class Program
{
    public static void Main(string[] args)
    {
        // configuration and arguments
        var config = Config.ReadConfig();
        var argv = new Args(args);

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
        context.LastAccessedAndCurrentCount_ToNewCount = Bump;

        var store = new SqliteStore(context);
        // run actual command and return result to sdout
        var command = SelectCommand(argv, context);
        try
        {
            var result = command.Invoke(store);
            if (result.Success)
                Console.WriteLine(result.Value);
        }
        catch (Exception e)
        {
            Console.WriteLine("failed" + e);
        }
    }

    public static Func<IStore, Result> SelectCommand(Args args, Context context)
    {
        return args.Name switch
        {
            Args.Names.EMPTY => (_) => 
            {
                string possibleArgs = string.Join(" ", Args.Names.CD, Args.Names.Bump, Args.Names.Init, Args.Names.List, Args.Names.Exec);
                return Result.Ok($"Please provide a command: {possibleArgs}");
            },
            Args.Names.CD => (store) =>
            {
                context.Log($"ChangeDirectoryCommand: {args.Name} {string.Join(" ", args)}");

                var dir = context.GetUserDir();
                var lookup = store.Lookup(args);

                return lookup is not null ? Result.Ok(lookup) : Result.Ok(dir);
            },

            Args.Names.List => (store) =>
            {
                context.Log($"ListCommand: {args.Name} {string.Join(" ", args)}");
                var list = store.List();
                return Result.Ok(list);
            },

            Args.Names.Init => (store) =>
            {
                context.Log($"InitCommand: {args.Name} {string.Join(" ", args)}");
                store.Touch();

                return Result.Ok("initialized store");
            },
            Args.Names.Bump => (store) => 
            {
                 store.Bump(args.First()); 
                 return Result.Ok(string.Empty); 
            },
            Args.Names.Exec => (store) =>
            {
                context.Log($"ExecCommand: {args.Name} {string.Join(" ", args)}");
                var lookup = new Exec();
                lookup.Run(args.First(), context);

                return Result.Ok("");
            },
            

            _ => (_) => Result.Fail()
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