namespace Apartments.Domain.Services.AccountService.Results;

public abstract record CreateResult;

public sealed record SuccessCreateResult : CreateResult;

public sealed record ErrorCreateResult : CreateResult
{
    public IEnumerable<string> ErrorMessage { get; init; } = [];
}
