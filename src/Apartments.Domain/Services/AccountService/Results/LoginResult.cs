namespace Apartments.Domain.Services.AccountService.Results;

public abstract record LoginResult;

public sealed record LoginSuccessResult : LoginResult;

public sealed record UserLockOutFoundLoginResult : LoginResult;

public sealed record UserNotFoundLoginResult : LoginResult;

public sealed record InvalidPasswordLoginResult : LoginResult;
