using Apartments.Domain.Services.Apartments.Dtos;

namespace Apartments.Domain;

public class Result
{
    public bool IsFailed => !IsSuccess;

    public bool IsSuccess { get; init; }

    public IEnumerable<string> Errors { get; init; } = Array.Empty<string>();

    public static Result Ok() => 
        new() { IsSuccess = true };

    public static Result Fail(IEnumerable<string> errors) => 
        new() { IsSuccess = false, Errors = errors };
}

public class Result<T>
    : Result
{
    public Result()
    {
    }
    
    public Result(T value)
    {
        Value = value;
    }
    
    public T? Value { get; }
    
    public static Result<T> Ok(T value) => 
        new(value) { IsSuccess = true };
    
    public new static Result<T> Fail(IEnumerable<string> errors) => 
        new() { IsSuccess = false, Errors = errors };
    
}
