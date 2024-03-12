using Apartments.Application.Modules.AccountServices.Models;

namespace Apartments.Application.Modules.AccountServices.Dtos;

public record struct CreateRequestCommand(UserName UserName, Password Password);
