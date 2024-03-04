using System.Text;
using autojump.Store;

namespace autojump;

public class Exec
{
    public void Run(string sql, Context context)
    {
        exec(sql, context);
    }

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