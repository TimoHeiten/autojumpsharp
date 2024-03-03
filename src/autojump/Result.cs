namespace autojump;

/// <summary>
/// The result of a command
/// </summary>
public sealed record Result
{
    public string? Value { get; private set; }
    public bool Success => !string.IsNullOrWhiteSpace(Value);

    private Result()
    { }

    public static Result Ok(string value) => new() { Value = value };
    public static Result Fail() => new();
}