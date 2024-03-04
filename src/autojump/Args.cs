using System.Collections;
using System.Diagnostics;

namespace autojump;

// show name and joined args
[DebuggerDisplay("{Name} + {string.Join(\",\", _args)}")]
public readonly struct Args : IEnumerable<string>
{
    /// <summary>
    /// The identified command name
    /// </summary>
    public string Name { get; }
    private readonly ArraySegment<string> _args;

    public Args(string[] args)
    {
        if (args.Length == 0 || args[0].Contains("help", StringComparison.InvariantCultureIgnoreCase))
        {
            Name = "EMPTY";
            _args = Array.Empty<string>();
        }
        else
        {
            Name = args[0].ToLowerInvariant();
            _args = new ArraySegment<string>(args, 1, args.Length - 1);
        }
    }

    public IEnumerator<string> GetEnumerator()
        => Name != "FAIL" 
            ? _args.GetEnumerator() 
            : Enumerable.Empty<string>().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

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