using Apartments.Application.Apartments;
using Apartments.Infrastructure.Apartments.Extensions;

namespace Apartments.WebApi.Extensions;

public static class ApplicationModuleExtensions
{
    public static IServiceCollection AddApartmentsModule(this IServiceCollection services)
    {
        services.AddApartmentsInfrastructureServices();
        services.AddApartmentsApplicationServices();
        
        return services;
    }
}
