namespace Apartments.WebApi.Requests;

public class Result
{
    public bool IsFailed { get; init; }
        
    public bool IsSuccess { get; init; }

    public IEnumerable<string> Errors { get; init; } = Array.Empty<string>();

    public static Result Ok()
    {
        return new Result() { IsSuccess = true };
    }
}

public class Result<T>(T value) 
    : Result
{
    public T Value { get; } = value;

    public static Result<T> Ok(T value) => new(value) {  IsSuccess = true };
}
