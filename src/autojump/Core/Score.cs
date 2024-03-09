using System;

namespace autojump.Core;

/// <summary>
/// Holds the algorithm to bump the score of a directory
/// </summary>
public sealed class Score
{
    public static double Bump(DateTime lastAccessed, double currentCount)
    {
        var now = DateTime.UtcNow;
        var diff = now - lastAccessed;
        if (diff < TimeSpan.FromHours(1))
        {
            return currentCount * 4;
        }
        if (diff < TimeSpan.FromDays(1))
        {
            return currentCount * 2;
        }
        if (diff < TimeSpan.FromDays(7))
        {
            return currentCount;
        }
        if (diff < TimeSpan.FromDays(30))
        {
            return currentCount * 0.5;
        }
        if (diff < TimeSpan.FromDays(365))
        {
            return currentCount * 0.25;
        }
        return currentCount / 8;
    }
}