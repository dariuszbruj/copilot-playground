using Apartments.Domain;
using Apartments.Domain.Services.Apartments;
using Apartments.Infrastructure.Apartments.Models;
using Apartments.Infrastructure.Apartments.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Apartments.Infrastructure.Apartments.Extensions;

public static class ApartmentsDbModelExtensions
{
    public static Apartment ToApartment(this ApartmentDbModel dbModel)
    {
        var apartment = new Apartment
        {
            Id = dbModel.Guid,
            Name = dbModel.Name,
            Address = dbModel.Address is not null 
                ? new Address()
                    {
                        BuildingNo = dbModel.Address.BuildingNumber,
                        FlatNumber = dbModel.Address.FlatNumber,
                        Street = dbModel.Address.Street,
                        City = dbModel.Address.City,
                        State = dbModel.Address.State,
                        ZipCode = dbModel.Address.ZipCode
                    }
                : Address.Empty
        };

        return apartment;
    }

    public static ApartmentDbModel FromDomainModel(Apartment domainModel) =>
        new()
        {
            Guid = domainModel.Id,
            Name = domainModel.Name,
            Address = new ApartmentAddressDbModel()
            {
                BuildingNumber = domainModel.Address.BuildingNo,
                FlatNumber = domainModel.Address.FlatNumber,
                Street = domainModel.Address.Street,
                City = domainModel.Address.City,
                State = domainModel.Address.State,
                ZipCode = domainModel.Address.ZipCode
            }
        };
}

public static class ApartmentsModuleExtensions
{
    public static IServiceCollection AddApartmentsInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IApartmentRepository, ApartmentRepository>();

        return services;
    }
}
