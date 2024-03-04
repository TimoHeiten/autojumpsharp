using System.Data.SQLite;

namespace autojump.Store;

public sealed partial class SqliteStore
{
    public SQLiteConnection TouchStore()
    {
        // create a new sqlite db if none exists at the user directory + autojump.db
        var path = _context.Configuration.ConnectionPath;

        bool isInitial = !File.Exists(path);
        if (isInitial)
        {
            _context.Log($"Creating new store: {path}");
            SQLiteConnection.CreateFile(path);
        }

        var connection = new SQLiteConnection($"Data Source={path}");
        connection.Open();

        if (isInitial)
        {
            _context.Log($"setup the table(s) at: {path}");
            using var command = connection.CreateCommand();
            command.CommandText = "CREATE TABLE locations (id INTEGER PRIMARY KEY, path TEXT NOT NULL, count REAL NOT NULL, last_accessed DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP)";
            command.ExecuteNonQuery();
        }

        return connection;
    }

    public void Touch() => TouchStore();
}