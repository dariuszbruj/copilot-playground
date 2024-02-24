using Apartments.Domain.Services.AccountService.Models;

namespace Apartments.Domain.Services.AccountService.Dtos;

public sealed record LoginRequestDto(UserName UserName, Password Password, LockoutOnFailure LockoutOnFailure);
