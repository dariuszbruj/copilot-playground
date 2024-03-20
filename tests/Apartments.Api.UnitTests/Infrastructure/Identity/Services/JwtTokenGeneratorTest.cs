using Apartments.Infrastructure.Identity.Services;
using FakeItEasy;
using Microsoft.Extensions.Options;

namespace Apartment.Api.UnitTests.Infrastructure.Identity.Services;

public class JwtTokenGeneratorTests
{
    [Fact]
    public void GenerateToken_ShouldReturnValidJwtToken()
    {
        // Arrange
        var username = "testuser";
        var key = "testkey-that-should-be-longer-than-16-characters";
        var utcNow = new DateTime(2024, 02, 24, 12, 49, 52, DateTimeKind.Utc);
        var expirationTime = TimeSpan.FromMinutes(30);
        var timeProviderFake = A.Fake<TimeProvider>();
        A.CallTo(() => timeProviderFake.GetUtcNow())
            .Returns(utcNow);
        var optionsFake = A.Fake<IOptions<JwtTokenGeneratorOptions>>();
        A.CallTo(() => optionsFake.Value)
            .Returns(new JwtTokenGeneratorOptions { Key = key, ExpirationTime = expirationTime });
            
        var tokenGenerator = new JwtTokenGenerator(timeProviderFake, optionsFake);
            
        const string expectedJwtToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbm"
            + "FtZSI6InRlc3R1c2VyIiwibmJmIjoxNzA4Nzc4OTkyLCJleHAiOjE3MDg3ODA3OTIsImlhdCI6MTc"
            + "wODc3ODk5Mn0.vazhpbPeEaOchMLbujgIOLxy6IsrSgGOnIV66B4CF_0";

        // Act
        var jwtToken = tokenGenerator.GenerateToken(username);

        // Assert
        Assert.Equal(expectedJwtToken, jwtToken);
    }
}
