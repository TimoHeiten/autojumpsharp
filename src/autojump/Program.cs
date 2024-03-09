using autojump.Core;
using autojump.Input;
using autojump.Store;

public static class Program
{
    public static void Main(string[] args)
    {
        // configuration and arguments
        var argv = new Args(args);
        var context = Context.Create(Config.ReadConfig(), Score.Bump);
        var store = new SqliteStore(context);

        try
        {
            string result = RunApp(argv, context, store);
            Console.WriteLine(result);
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed " + e.Message);
        }
    }

    public static string RunApp(Args args, Context context, SqliteStore store)
    {
        // run actual command and return result
        var command = Commands.SelectCommand(args, context);

        var result = command.Invoke(store);
        return result.Success ? result.Value! : string.Empty;
    }
}