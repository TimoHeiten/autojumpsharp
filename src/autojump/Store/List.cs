namespace autojump.Store;

public sealed partial class SqliteStore
{
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