using Apartments.Domain.Services.AccountServices.Dtos;

namespace Apartments.Domain.Services.AccountServices;

public interface IAccountService
{
    Task<Result> CreateAsync(CreateRequestDto requestDto, CancellationToken cancellationToken = default);

    Task<Result> LoginAsync(LoginRequestDto requestDto, CancellationToken cancellationToken = default);
    
}
