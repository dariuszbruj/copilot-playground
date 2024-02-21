using Apartments.Infrastructure.EntityFramework.Contexts;
using Apartments.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Apartments.Infrastructure.Identity;

public static class IdentityServiceExtension
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services)
    {
        services.AddIdentityCore<User>(options => { options.User.RequireUniqueEmail = true; })
            .AddEntityFrameworkStores<IdentityDbContext>()
            .AddSignInManager()
            .Services
            .AddSingleton(TimeProvider.System);

        return services;
    }
}
