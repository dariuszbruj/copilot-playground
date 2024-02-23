namespace Apartments.Domain.Services.AccountService;

public abstract record CreateResult;

public sealed record CreateSuccessResult : CreateResult;

public sealed record CreateErrorResult : CreateResult
{
    public IEnumerable<string> ErrorMessage { get; init; } = [];
}
