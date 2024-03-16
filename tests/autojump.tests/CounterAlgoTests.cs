using System;
using autojump.Core;
using FluentAssertions;
using Xunit;

namespace autojump;

public sealed class CounterAlgoTests
{
    private void Execute(DateTime lastAccessed, double expected)
    {
        // Arrange
        var currentCount = 1;
        
        // Act
        var result = Score.Bump(lastAccessed, currentCount);
        
        // Assert
        result.Should().Be(expected);
    }
    
    [Fact]
    public void Bump_WhenLastAccessedIsLessThanAnHourAgo_ReturnsCurrentCountTimesFour()
        => Execute(DateTime.Now.AddMinutes(-30), 4);

    [Fact]
    public void Bump_WhenLastAccessedIsLessThanADayAgo_ReturnsCurrentCountTimesTwo()
        => Execute(DateTime.Now.AddHours(-12), 2);
    
    [Fact]
    public void Bump_WhenLastAccessedIsLessThanAWeekAgo_ReturnsCurrentCount()
        => Execute(DateTime.Now.AddDays(-3), 1);
    
    
    [Fact]
    public void Bump_WhenLastAccessedIsMoreThanAWeekAgo_ReturnsCurrentCount()
        => Execute(DateTime.Now.AddDays(-8), 0.5);
    
    [Fact]
    public void Bump_WhenLastAccessedIsMoreThanADayAgo_ReturnsCurrentCount()
        => Execute(DateTime.Now.AddDays(-2), 1);
    
    [Fact]
    public void Bump_WhenlastAccessedMoreThanAYearAgo_ReturnsCurrentCount()
        => Execute(DateTime.Now.AddYears(-2), 0.125);
}