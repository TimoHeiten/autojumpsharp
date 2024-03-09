using System.Data.SQLite;
using autojump.Core;

namespace autojump.Store;
/// <summary>
/// <inheritdoc cref="IStore"/>
/// <para></para>
/// Sqlite version of the store 
/// </summary>
public sealed partial class SqliteStore : IStore
{
    private readonly Context _context;
    public SqliteStore(Context context)
        => _context = context;

    public readonly record struct Location(long Id, string? Path, DateTime LastAccessed, double Count)
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