using Apartments.Domain.Services.AccountService.Dtos;
using Apartments.Domain.Services.AccountService.Results;

namespace Apartments.Domain.Services.AccountService;

public interface IAccountService
{
    Task<CreateResult> CreateAsync(CreateRequestDto requestDto, CancellationToken cancellationToken = default);

    Task<LoginResult> LoginAsync(LoginRequestDto requestDto, CancellationToken cancellationToken = default);
    
}
