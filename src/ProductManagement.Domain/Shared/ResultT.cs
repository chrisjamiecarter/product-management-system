namespace ProductManagement.Domain.Shared;

/// <summary>
/// Represents the outcome of an operation with a value type, including success or failure and associated error.
/// </summary>
/// <typeparam name="TValue">The type of the value.</typeparam>
public class Result<TValue> : Result
{
    private readonly TValue? _value;

    protected internal Result(TValue? value, bool isSuccess, Error error) : base(isSuccess, error)
    {
        _value = value;
    }

    public TValue Value => IsSuccess ? _value! : throw new InvalidOperationException("The value of a failure result cannot be accessed.");

    public static implicit operator Result<TValue>(TValue? value) => Create(value);
}
