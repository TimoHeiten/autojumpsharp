using System;
using System.IO;
using FluentAssertions;
using Xunit;

namespace autojump;

public sealed class StoreTests
{
    private readonly Lookups _lookups;
    public StoreTests()
    {
        _lookups = Lookups.Default;
        var path = Path.Combine(Environment.CurrentDirectory, "autojump.db"); ;
        _lookups.Configuration =  new Configuration(path);
    }

    private void InsertData()
    {
        string cmdTxt(string path, double cnter = 1)
            => $"Insert into locations (path, count) values ('{path}', {cnter})";

        using var connection = SqliteStore.TouchStore(_lookups);
        using var command = connection.CreateCommand();

        foreach (var cmd in new[] {
            cmdTxt("G:\\repos", 8),
            cmdTxt("G:\\repos\\heitech\\lol"),
            cmdTxt("G:\\repos\\heitech\\lol"),
            cmdTxt("G:\\repos\\kunden\\v1\\kamel")
        })
        {
            command.CommandText = cmd;
            command.ExecuteNonQuery();
        }
    }

    [Theory]
    [InlineData("repos", @"G:\repos")]
    [InlineData("kamel", @"G:\repos\kunden\v1\kamel")]
    [InlineData("lol", @"G:\repos\heitech\lol")]
    [InlineData("heitech", @"G:\repos\heitech\lol")]
    public void Lookup_Returns_ExpectedDataFromSqliteFile(string arg, string expected)
    {
        // Arrange
        InsertData();

        // Act
        var result = SqliteStore.Lookup(new[] { arg }, _lookups);
        
        // Assert
        result.Should().Be(expected);
    }
}