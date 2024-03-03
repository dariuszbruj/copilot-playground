using Microsoft.Extensions.DependencyInjection;

namespace Apartments.Application.Apartments;

public static class ApartmentsModuleExtensions
{
    public static IServiceCollection AddApartmentsServices(this IServiceCollection services)
    {
        services.AddScoped<ApartmentService>();

        return services;
    }
}
