using System.Data.SQLite;

namespace autojump.Store;

public sealed partial class SqliteStore 
{
    /// <summary>
    /// Look for the presented path segments in the store and return the best match while bumping it up simultaenously.
    /// </summary>
    /// <param name="args"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public string? Lookup(IEnumerable<string> args)
    {
        using var connection = TouchStore();
        var location = GetValuesForMatch(args, connection);
        BumpPath(location, connection);

        return location.Path;
    }

    private static Location GetValuesForMatch(IEnumerable<string> names, SQLiteConnection connection)
    {
        string orLikePathParameter = "";
        foreach (var name in names)
            orLikePathParameter += $"path LIKE '%{name}%' OR ";

        orLikePathParameter = orLikePathParameter.Substring(0, orLikePathParameter.Length - 4);

        using var command = connection.CreateCommand();
        command.CommandText = $"SELECT id, path, last_accessed, count FROM locations WHERE {orLikePathParameter} ORDER BY count DESC LIMIT 1";

        using var reader = command.ExecuteReader();
        if (!reader.HasRows)
            return Location.Empty;

        while (reader.Read())
            return Location.FromReader(reader);

        return Location.Empty;
    }
    
    public string List()
    {
        using var connection = TouchStore();
        
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT id, path, last_accessed, count FROM locations";

        using var reader = command.ExecuteReader();
        if (!reader.HasRows)
            return Location.Empty.Stringify();

        Location firstRow = new(0, "id - path - last_accessed - count", DateTime.MinValue, 0.0);
        var locations = new List<Location> { firstRow };

        while (reader.Read())
            locations.Add(Location.FromReader(reader));

        return string.Join(Environment.NewLine, locations.Select(l => l.Stringify()));
    }
}