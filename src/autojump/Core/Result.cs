namespace autojump.Core;

/// <summary>
/// The result of a autojump-command
/// </summary>
public sealed record Result
{
    /// <summary>
    /// The actual result
    /// </summary>
    public string? Value { get; private set; }
    /// <summary>
    /// Indicates if the Value holds any Value
    /// </summary>
    public bool Success { get; private set; }

    private Result()
    { }

    public static Result Ok(string value) => new() { Value = value, Success = true };
    public static Result Fail() => new() { Value = "FAILED", Success = false };
}