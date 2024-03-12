using Apartments.Application.Modules.AccountServices;
using Apartments.Application.Modules.Tokens;
using Apartments.Infrastructure.EntityFramework.Contexts;
using Apartments.Infrastructure.Identity;
using Apartments.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Apartment.Api.UnitTests.Infrastructure.Identity;

public class IdentityServiceExtensionTests
{
    [Fact]
    public void AddIdentityServices_ShouldRegisterServices()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder().Build();

        services.AddDbContext<IdentityDbContext>(options =>
            options.UseInMemoryDatabase("TestDb"));

        services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<IdentityDbContext>();

        // Act
        services.AddIdentityServices(configuration);

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        Assert.NotNull(serviceProvider.GetService<UserManager<User>>());
        Assert.NotNull(serviceProvider.GetService<SignInManager<User>>());
        Assert.NotNull(serviceProvider.GetService<IAuthenticationService>());
        Assert.NotNull(serviceProvider.GetService<IAuthorizationService>());
        Assert.NotNull(serviceProvider.GetService<ITokenGenerator>());
        Assert.NotNull(serviceProvider.GetService<IAccountService>());
    }

    [Fact]
    public void AddIdentityServices_ShouldConfigureOptions()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder().Build();

        // Act
        services.AddIdentityServices(configuration);

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var options = serviceProvider.GetRequiredService<IOptions<IdentityOptions>>();
        Assert.True(options.Value.User.RequireUniqueEmail);
    }

    [Fact]
    public void AddIdentityServices_ShouldAddAuthentication()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder().Build();

        // Act
        services.AddIdentityServices(configuration);

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var authenticationOptions = serviceProvider.GetRequiredService<IOptions<AuthenticationOptions>>();
        Assert.Equal(JwtBearerDefaults.AuthenticationScheme, authenticationOptions.Value.DefaultAuthenticateScheme);
        Assert.Equal(JwtBearerDefaults.AuthenticationScheme, authenticationOptions.Value.DefaultChallengeScheme);
    }

    [Fact]
    public async Task AddIdentityServices_ShouldAddJwtBearerAuthentication()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder().Build();

        // Act
        services.AddIdentityServices(configuration);

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var authenticationSchemeProvider = serviceProvider.GetRequiredService<IAuthenticationSchemeProvider>();
        Assert.Contains(await authenticationSchemeProvider
            .GetAllSchemesAsync(), scheme => scheme.Name == JwtBearerDefaults.AuthenticationScheme);
    }
}
