using System;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using autojump.Core;
using autojump.Store;
using FluentAssertions;
using Xunit;

namespace autojump;

/// <summary>
/// works fine, but still probably best to run this manually
/// </summary>
public sealed class IntegrationTests : IDisposable
{

    // todo -> does not work since the sql connection is not closed -.- ... but it is? another time then
    // [Fact]
#pragma warning disable xUnit1004
    [Fact(Skip = "This is a full cycle test and should be run manually - with a ENV set etc.")] 
#pragma warning restore xUnit1004
    public async Task Run_Full_Cycle()
    {
        // todo remove and and put into e2escript
        Environment.SetEnvironmentVariable("AUTOJUMP_DB_PATH", "G:\\tools\\autojump.db");
        // --------------------------------
        // Phase 1 - touch data store
        // --------------------------------
        Program.Main(new[] {"init"});
        await CheckDb(async con =>
        {
            await using (var command = con.CreateCommand())
            {
                command.CommandText = "select count(*) from locations";
                var result = command.ExecuteScalar();
                result.Should().Be(0);
            }
        });

        // --------------------------------
        // Phase 2 - insert multiple directories
        // --------------------------------
        Enumerable.Range(0, 10).ToList().ForEach(x =>
            Program.Main(new[] {"bump", $"path{x}"})
        );
        await CheckDb(async con =>
        {
            await using (var command = con.CreateCommand())
            {
                command.CommandText = "select count(*) from locations";
                var result = command.ExecuteScalar();
                result.Should().Be(10);
            }
        });

        // --------------------------------
        // Phase 3 - Bump 2 of them
        // --------------------------------
        Program.Main(new [] { "bump", "path1" });
        Program.Main(new [] { "bump", "path8" });

        await CheckDb(async c =>
        {
            await using (var cmd = c.CreateCommand())
            {
                cmd.CommandText = "SELECT id, path, last_accessed, count FROM locations WHERE path = 'path1' OR path = 'path8'";
                var reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                {
                    return;
                }

                while (reader.Read())
                {
                    var location = SqliteStore.Location.FromReader(reader);
                    switch (location.Path)
                    {
                        case "path1":
                        case "path8":
                            location.Count.Should().Be(4); // since it was accessed in the last our
                            break;
                    }
                }
            }
        });

        // --------------------------------
        // Phase 4 - show matched result (if any)
        // --------------------------------
        async Task CheckForPath(string path, string expected)
        {
            await using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                Program.Main(new [] { "cd", path});
                var result = sw.ToString();
                result.Should().Contain(expected);
            }
        }
        await CheckForPath("1", "path1");
        await CheckForPath("8", "path8");
        var userPath = Context.Default.GetUserDir();
        await CheckForPath("abcaffeschnee", userPath);
    }

    private static async Task CheckDb(Func<SQLiteConnection, Task> action)
    {
        await using var connection = new SQLiteConnection("Data Source="+ Environment.GetEnvironmentVariable("AUTOJUMP_DB_PATH"));
        await connection.OpenAsync();
        try
        {
            await action(connection);
        }
        catch (Exception e)
        {
            Assert.False(true, e.Message);
        }
        finally
        {
            connection.Close();
        }
    }

    public void Dispose()
    {
        var path = Environment.GetEnvironmentVariable("AUTOJUMP_DB_PATH");
        if (string.IsNullOrWhiteSpace(path))
        {
            return;
        }

        try
        {
            File.Delete(path);
        }
        catch 
        {
            // intentionally left blank, since the test in general was a success if we end up here i guess
        }
    }
}