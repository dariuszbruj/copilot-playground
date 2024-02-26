using Apartments.Domain;
using Apartments.Infrastructure.Apartments.Models;

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
