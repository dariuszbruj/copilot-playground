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
            
        var expectedJwtToken =
            "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6InRlc3R1c2VyIiwibmJmIjoxNzA4Nzc1Mzky"
            + "LCJleHAiOjE3MDg3NzcxOTIsImlhdCI6MTcwODc3NTM5Mn0.CZn9hMs93PiYe5hlKrIh8-Nz9uD6cB_V1I_X_7P09qQ";

        // Act
        var jwtToken = tokenGenerator.GenerateToken(username);

        // Assert
        Assert.Equal(expectedJwtToken, jwtToken);
    }
}
