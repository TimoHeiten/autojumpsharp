using System;
using System.IO;
using autojump.Core;
using autojump.Input;
using autojump.Store;
using FluentAssertions;
using Xunit;

namespace autojump;

public sealed class StoreTests
{
    private readonly Context _context;
    private readonly SqliteStore _sut;
    public StoreTests()
    {
        _context = Context.Default;
        var path = Path.Combine(Environment.CurrentDirectory, "autojump.db"); ;
        _context.Configuration =  new Configuration(path);
        
        _sut = new SqliteStore(_context);
    }

    private void InsertData()
    {
        string cmdTxt(string path, double cnter = 1)
            => $"Insert into locations (path, count) values ('{path}', {cnter})";

        using var connection = _sut.TouchStore();
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
        var result = _sut.Lookup(new[] { arg });
        
        // Assert
        result.Should().Be(expected);
    }
}