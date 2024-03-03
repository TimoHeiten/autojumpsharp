
using System;
using System.IO;
using FluentAssertions;
using Xunit;

namespace autojump;

public sealed class ParseTests
{
    private readonly Lookups _lookups;

    public ParseTests()
    {
        _lookups = Lookups.Default;
        var path = Path.Combine(Environment.CurrentDirectory, "test-log.log");
        _lookups.GetUserDir = () => @"G:\Repos\autojumpSharp\tests";
        _lookups.Log = msg =>
        {
            var entry = $"{DateTime.Now} - {msg}{Environment.NewLine}";
            File.AppendAllText(path, entry);
        };
    }

    [Theory]
    [InlineData("cd")]
    [InlineData("cD")]
    [InlineData("Cd")]
    [InlineData("CD")]
    public void SelectCommand_WithCorrectArgs_ReturnsChangeDirWithUserDirLookupOnDefaultLookups(string cd)
    {
        // Arrange
        var args = new Args(new [] { cd });

        // Act
        var func = Program.SelectCommand(args, _lookups);
        var results = func.Invoke();

        // Assert
        results.Success.Should().BeTrue();
        results.Value.Should().Be(@"G:\Repos\autojumpSharp\tests");
    }

    [Fact]
    public void SelectCommand_WithNMatchingArgs_Returns_ResultFail()
    {
        // Arrange
        var args = new Args(new[] { "no match" });
        var sut = Program.SelectCommand(args, _lookups);

        // Act
        var command = sut;

        // Assert
        Result r = command.Invoke();
        r.Success.Should().BeFalse();
    } 
}