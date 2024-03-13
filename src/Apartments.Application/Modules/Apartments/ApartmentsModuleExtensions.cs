using Microsoft.Extensions.DependencyInjection;

namespace Apartments.Application.Modules.Apartments;

public static class ApartmentsModuleExtensions
{
    public static IServiceCollection AddApartmentsApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IApartmentService, ApartmentService>();

        return services;
    }
}
