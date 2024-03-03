namespace autojump;

public sealed class Result
{
    public string? Value { get; private set; }
    public bool Success => !string.IsNullOrWhiteSpace(Value);

    private Result()
    { }

    public static Result Ok(string value) => new() { Value = value };
    public static Result Fail() => new();

    public override string ToString()
    {
        return $"'Result' {Value} {Success}";
    }
}