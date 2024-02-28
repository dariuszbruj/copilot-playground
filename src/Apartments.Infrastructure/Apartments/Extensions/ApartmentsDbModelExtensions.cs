using Apartments.Domain;
using Apartments.Domain.Services.Apartments;
using Apartments.Infrastructure.Apartments.Models;
using Apartments.Infrastructure.Apartments.Repositories;
using Apartments.Infrastructure.EntityFramework.Contexts;
using Microsoft.Extensions.DependencyInjection;

namespace Apartments.Infrastructure.Apartments.Extensions;

public static class ApartmentsDbModelExtensions
{
    public static Apartment ToApartment(this ApartmentDbModel dbModel)
    {
        var apartment = new Apartment
        {
            Id = dbModel.Guid,
            Name = dbModel.Name
        };

        return apartment;
    }

    public static ApartmentDbModel FromDomainModel(Apartment domainModel) => 
        new ApartmentDbModel{ Guid = domainModel.Id, Name = domainModel.Name };
}

public static class ApartmentsModuleExtensions
{
    public static IServiceCollection AddApartmentsInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IApartmentRepository, ApartmentRepository>();

        return services;
    }
}
