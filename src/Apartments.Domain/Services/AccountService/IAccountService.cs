namespace Apartments.Domain.Services.AccountService;

public interface IAccountService
{
    Task<CreateResult> CreateAsync(CreateRequestDto requestDto);

    Task<LoginResult> LoginAsync(LoginRequestDto requestDto);
    
}
