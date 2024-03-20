using Microsoft.Extensions.DependencyInjection;

namespace Apartments.Application.Modules.Utilities;

public static class UtilitiesModuleExtensions
{
    public static IServiceCollection AddUtilitiesApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IUtilityService, UtilityService>();

        return services;
    }
}
