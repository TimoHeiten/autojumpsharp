using autojump.Core;

namespace autojump.Store;

public sealed class SqliteStoreConfiguration : IStoreConfiguration
{
    public string ConnectionPath()
    {
        var defaultPath = Context.Default.GetUserDir();
        return Path.Combine(defaultPath, "autojump", "autojump.db");
    }

    public string DatabaseName()
        =>  "autojump.db";
}