using Apartments.Domain.Services.AccountServices.Models;

namespace Apartments.Domain.Services.AccountServices.Dtos;

public record struct CreateRequestDto(UserName UserName, Password Password);
