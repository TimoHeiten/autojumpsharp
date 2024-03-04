using System.Data.SQLite;

namespace autojump.Store;

public sealed partial class SqliteStore : IStore
{
    private readonly Context _context;
    public SqliteStore(Context context)
        => _context = context;

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

    private readonly record struct Location(long Id, string? Path, DateTime LastAccessed, double Count)
    {
        public static Location Empty => new(-1, null, DateTime.MinValue, 0);
        
        public static Location FromReader(SQLiteDataReader reader)
        {
            var id = (long)reader["id"];
            var lastAccessed = (DateTime)reader["last_accessed"];
            var count = (double)reader["count"];
            var path = (string)reader["path"];

            return new(id, path, lastAccessed, count);
        }
        
        public string Stringify() => $"{Id} {Path} {LastAccessed.ToShortDateString()} {Count}";
    }
}