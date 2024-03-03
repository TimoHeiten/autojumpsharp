using System.Collections;
using System.Diagnostics;

namespace autojump;

// show name and joined args
[DebuggerDisplay("{Name} + {string.Join(\",\", _args)}")]
public readonly struct Args : IEnumerable<string>
{
    private readonly ArraySegment<string> _args;
    public string Name { get; }

    public Args(string[] args)
    {
        if (args.Length == 0)
        {
            Name = "FAIL";
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

        public static string[] All { get; } = { CD, Bump };
    }
}