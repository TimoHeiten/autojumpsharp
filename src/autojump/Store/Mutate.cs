using System.Data;
using System.Data.SQLite;

namespace autojump.Store;

public sealed partial class SqliteStore
{
    public SQLiteConnection TouchStore()
    {
        // create a new sqlite db if none exists at the user directory + autojump.db
        var path = _storeConfiguration.ConnectionPath();
        
        bool isInitial = !File.Exists(path);
        if (isInitial)
        {
            _context.Log($"Creating new store: {path}");
            File.Create(path).Close();
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

    public void Touch()
    {
        var con = TouchStore();
        con.Close();
    } 

    public void Bump(string path)
    {
        using var connection = TouchStore();

        var location = GetValuesForMatch(new[] { path }, connection);
        if (location.Id != -1)
        {
            BumpPath(location, connection);
            return;
        }

        var insert = $"INSERT INTO locations (path, last_accessed, count) VALUES ('{path}', CURRENT_TIMESTAMP, 1)";
        using var command = connection.CreateCommand();
        command.CommandText = insert;
        command.ExecuteNonQuery();
    }

    private void BumpPath(Location location, SQLiteConnection connection)
    {
        var (id, path, lastAccessed, count) = location;
        var newCount = _context.LastAccessedAndCurrentCount_ToNewCount(lastAccessed, count);

        using var command = connection.CreateCommand();
        command.CommandText = $"UPDATE locations SET count = {newCount}, last_accessed = CURRENT_TIMESTAMP WHERE id = {id}";
        command.ExecuteNonQuery();
    }
}