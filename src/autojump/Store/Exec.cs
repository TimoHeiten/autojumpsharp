using System.Text;
using autojump.Core;
using autojump.Store;

namespace autojump;

/// <summary>
/// Run a sql command against the store. Sure this seems like a risk. But consider that this is dev only tool with no access to the web.
/// And if there is someone on your machine already, this tool with sql injection to a teeny tiny sqlite store wont kill ya then.
/// </summary>
public class Exec
{
    /// <summary>
    /// run the sql command or query
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="context"></param>
    public void Run(string sql, Context context)
        => exec(sql, context);

    /// <summary>
    /// virtual as a test seam
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="context"></param>
    protected virtual void exec(string sql, Context context)
    {
        var store = new SqliteStore(context);
        using var connection = store.TouchStore();
        using var command = connection.CreateCommand();
        command.CommandText = sql;

        try
        {
            if (!sql.Contains("SELECT", StringComparison.InvariantCultureIgnoreCase))
            {
                var result = command.ExecuteNonQuery();
                context.Log($"exec result - affected rows: '{result}'");
                return;
            }
            var reader = command.ExecuteReader();
            var stringBuilder = new StringBuilder();
            if (!reader.HasRows)
            {
                context.Log("no results");
                return;
            }

            while (reader.Read())
            {
                stringBuilder.Clear();

                var count = reader.FieldCount;
                for (var i = 0; i < count; i++)
                {
                    stringBuilder.Append(reader[i]);
                }

                context.Log(stringBuilder.ToString());
            }
        }
        catch (Exception e)
        {
            context.Log(e.Message);
        }
    }
}