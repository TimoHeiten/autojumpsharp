using System.Collections;
using System.Diagnostics;

namespace autojump.Input;

// show name and joined args
[DebuggerDisplay("{Name} + {string.Join(\",\", _args)}")]
public readonly struct Args : IEnumerable<string>
{
    /// <summary>
    /// The identified command name
    /// </summary>
    public string Name { get; }
    /// <summary>
    /// the other args, usually only one extra
    /// </summary>
    private readonly ArraySegment<string> _args;

    public Args(string[] args)
        => (Name, _args) = Parse(args);

    private static (string name, ArraySegment<string> parsedArgs) Parse(string[] args)
    {
        if (args.Length == 0 || args[0].Contains("help", StringComparison.InvariantCultureIgnoreCase))
        {
            return (Names.EMPTY, new ArraySegment<string>());
        }

        return (args[0].ToLowerInvariant(), new ArraySegment<string>(args, 1, args.Length - 1));
    }

    public IEnumerator<string> GetEnumerator()
        => Name != "FAIL" 
            ? _args.GetEnumerator() 
            : Enumerable.Empty<string>().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    /// <summary>
    /// All known command names
    /// </summary>
    public static class Names
    {
        public const string CD = "cd";
        public const string Bump = "bump";

        /// <summary>
        /// create the initial store
        /// </summary>
        public const string Init = "init";
        /// <summary>
        /// list all values with counter and last access
        /// </summary>
        public const string List = "list";
        /// <summary>
        /// run a query on the data store:O
        /// </summary>
        public const string Exec = "exec";

        public const string EMPTY = "EMPTY";
    }
}