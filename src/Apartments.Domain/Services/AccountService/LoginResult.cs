namespace Apartments.Domain.Services.AccountService;

public abstract record LoginResult;

public sealed record LoginSuccessResult : LoginResult;

public sealed record UserLockOutFoundResult : LoginResult;

public sealed record UserNotFoundResult : LoginResult;

public sealed record InvalidPasswordResult : LoginResult;
