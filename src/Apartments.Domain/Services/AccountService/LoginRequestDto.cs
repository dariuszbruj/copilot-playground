namespace Apartments.Domain.Services.AccountService;

public sealed record LoginRequestDto(UserName UserName, Password Password, LockoutOnFailure LockoutOnFailure);
