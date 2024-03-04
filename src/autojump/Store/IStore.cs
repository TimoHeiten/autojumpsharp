using System.Data.SQLite;

namespace autojump.Store;

public interface IStore
{
    void Touch();
    void Bump(string path);

    string List();
    string? Lookup(IEnumerable<string> args);
}