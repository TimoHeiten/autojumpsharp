
using System;
using System.Collections.Generic;
using System.IO;
using autojump.Core;
using autojump.Input;
using autojump.Store;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace autojump;

public sealed class ParseTests
{
    private readonly Context _context;
    private readonly IStore _store = Substitute.For<IStore>();
    public ParseTests()
    {
        _context = Context.Default;
        _store.Lookup(Arg.Any<IEnumerable<string>>()).Returns((string)null!);

        var path = Path.Combine(Environment.CurrentDirectory, "test-log.log");
        _context.GetUserDir = () => @"G:\Repos\autojumpSharp\tests";
        _context.Log = msg =>
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
        var func = Commands.SelectCommand(args, _context);
        var results = func.Invoke(_store);

        // Assert
        results.Success.Should().BeTrue();
        results.Value.Should().Be(@"G:\Repos\autojumpSharp\tests");
    }

    [Fact]
    public void SelectCommand_WithNMatchingArgs_Returns_ResultFail()
    {
        // Arrange
        var args = new Args(new[] { "no match" });
        var sut = Commands.SelectCommand(args, _context);

        // Act
        var command = sut;

        // Assert
        Result r = command.Invoke(_store);
        r.Success.Should().BeFalse();
    } 
}