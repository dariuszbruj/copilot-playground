using Apartments.Domain.Services.AccountServices.Models;

namespace Apartments.Domain.Services.AccountServices.Dtos;

public record struct LoginRequestDto(UserName UserName, Password Password, LockoutOnFailure LockoutOnFailure);
