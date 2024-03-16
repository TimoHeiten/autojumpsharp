using autojump.Input;

namespace autojump.Core;

/// <summary>
/// Holds the commands for the application
/// </summary>
public sealed class Commands
{
    /// <summary>
    /// Select a command according to the arguments. No fancy parsing. Just a simple switch - use it according to the readme me aye?
    /// </summary>
    /// <param name="args"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public static Func<IStore, Result> SelectCommand(Args args, Context context)
    {
        return args.Name switch
        {
            Args.Names.EMPTY => (_) => 
            {
                var possibleArgs = new [] { Args.Names.CD, Args.Names.Bump, Args.Names.Init, Args.Names.List, Args.Names.Exec};
                var inQuotes = string.Join(", ", possibleArgs.Select(x => $"'{x}'"));
                return Result.Ok($"Please provide one of the following commands: {inQuotes}");
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
}