//
// using System.Data.SQLite;
//
// namespace autojump;
//
// public static partial class SqliteStore : 
// {
//     /// <summary>
//     /// Look for the presented path segments in the store and return the best match while bumping it up simultaenously.
//     /// </summary>
//     /// <param name="args"></param>
//     /// <param name="context"></param>
//     /// <returns></returns>
//     public static string? Lookup(IEnumerable<string> args, Context context)
//     {
//         using var connection = TouchStore(context);
//         var location = GetValuesForMatch(args, connection);
//         BumpPath(location, connection, context);
//
//         return location.Path;
//     }
//
//     public static SQLiteConnection TouchStore(Context context)
//     {
//         // create a new sqlite db if none exists at the user directory + autojump.db
//         var path = context.Configuration.ConnectionPath;
//
//         bool isInitial = !File.Exists(path);
//         if (isInitial)
//         {
//             context.Log($"Creating new store: {path}");
//             SQLiteConnection.CreateFile(path);
//         }
//
//         var connection = new SQLiteConnection($"Data Source={path}");
//         connection.Open();
//
//         if (isInitial)
//         {
//             context.Log($"setup the table(s) at: {path}");
//             using var command = connection.CreateCommand();
//             command.CommandText = "CREATE TABLE locations (id INTEGER PRIMARY KEY, path TEXT NOT NULL, count REAL NOT NULL, last_accessed DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP)";
//             command.ExecuteNonQuery();
//         }
//
//         return connection;
//     }
//
//     private static Location GetValuesForMatch(IEnumerable<string> names, SQLiteConnection connection)
//     {
//         var orLikePathParameter = "";
//         foreach (var name in names)
//         {
//             orLikePathParameter += $"path LIKE '%{name}%' OR ";
//         }
//         orLikePathParameter = orLikePathParameter.Substring(0, orLikePathParameter.Length - 4);
//
//         using var command = connection.CreateCommand();
//         command.CommandText = $"SELECT id, path, last_accessed, count FROM locations WHERE {orLikePathParameter} ORDER BY count DESC LIMIT 1";
//
//         using var reader = command.ExecuteReader();
//         while (reader.Read())
//         {
//             return Location.FromReader(reader);
//         }
//
//         return Location.Empty;
//     }
//     private static void BumpPath(Location location, SQLiteConnection connection, Context context)
//     {
//         var (id, path, lastAccessed, count) = location;
//         var newCount = context.LastAccessedAndCurrentCount_ToNewCount(lastAccessed, count);
//
//         using var command = connection.CreateCommand();
//         command.CommandText = $"UPDATE locations SET count = {newCount}, last_accessed = CURRENT_TIMESTAMP WHERE id = {id}";
//         command.ExecuteNonQuery();
//     }
//
//     // todo list all values
//     // todo bump specific value oonly (without returning the path)
//
//
//     private readonly record struct Location(long Id, string? Path, DateTime LastAccessed, double Count)
//     {
//         public static Location Empty => new(-1, null, DateTime.MinValue, 0);
//         
//         public static Location FromReader(SQLiteDataReader reader)
//         {
//             var id = (long)reader["id"];
//             var lastAccessed = (DateTime)reader["last_accessed"];
//             var count = (double)reader["count"];
//             var path = (string)reader["path"];
//
//             return new(id, path, lastAccessed, count);
//         }
//     }
// }