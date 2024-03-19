namespace Apartments.Infrastructure.Identity.Services;

public class JwtTokenGeneratorOptions
{
    public string Key { get; init; } = string.Empty;
    public TimeSpan ExpirationTime { get; init; } = TimeSpan.FromMinutes(15);
    public string Issuer { get; set; }= string.Empty;
    
    public string Audience { get; set; }= string.Empty;
}
