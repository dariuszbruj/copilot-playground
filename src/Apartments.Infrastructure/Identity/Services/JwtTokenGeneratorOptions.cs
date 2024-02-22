namespace Apartments.Infrastructure.Identity.Services;

public class JwtTokenGeneratorOptions
{
    public string Key { get; init; } = string.Empty;
    public TimeSpan ExpirationTime { get; init; } = TimeSpan.FromMinutes(15);
}
