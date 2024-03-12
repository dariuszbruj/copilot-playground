using Apartments.Application.Modules.AccountServices.Models;

namespace Apartments.Application.Modules.AccountServices.Dtos;

public record struct LoginRequestCommand(UserName UserName, Password Password, LockoutOnFailure LockoutOnFailure);
