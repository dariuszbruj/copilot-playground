using Apartments.Application.Modules.Utilities;
using Apartments.Infrastructure.Utilities.Extensions;

namespace Apartments.WebApi.Extensions;

internal static class UtilityModuleExtensions
{
    public static IServiceCollection AddUtilityModule(this IServiceCollection services)
    {
        services.AddUtilityInfrastructureServices();
        services.AddUtilitiesApplicationServices();
        
        return services;
    }
}
