using Apartments.Application.Common;
using Apartments.Application.Modules.AccountServices.Dtos;

namespace Apartments.Application.Modules.AccountServices;

public interface IAccountService
{
    Task<Result> CreateAsync(CreateRequestCommand requestCommand, CancellationToken cancellationToken = default);

    Task<Result> LoginAsync(LoginRequestCommand requestCommand, CancellationToken cancellationToken = default);
    
}
