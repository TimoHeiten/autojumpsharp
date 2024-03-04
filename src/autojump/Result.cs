namespace autojump;

/// <summary>
/// The result of a command
/// </summary>
public sealed record Result
{
    public string? Value { get; private set; }
    public bool Success { get; private set; }

    private Result()
    { }

    public static Result Ok(string value) => new() { Value = value, Success = true };
    public static Result Fail() => new() { Value = "FAILED", Success = false };
}