using Apartments.Domain.Services.AccountService.Models;

namespace Apartments.Domain.Services.AccountService.Dtos;

public sealed record CreateRequestDto(UserName UserName, Password Password);
