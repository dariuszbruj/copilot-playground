using Apartments.Application.Modules.Apartments;
using Apartments.Infrastructure.Apartments.Extensions;

namespace Apartments.WebApi.Extensions;

internal static class ApplicationModuleExtensions
{
    public static IServiceCollection AddApartmentsModule(this IServiceCollection services)
    {
        services.AddApartmentsInfrastructureServices();
        services.AddApartmentsApplicationServices();
        
        return services;
    }
}
