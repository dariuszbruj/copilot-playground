using Apartments.Application.Modules.Utilities;
using Apartments.Domain.Models;
using Apartments.Infrastructure.Utilities.Models;
using Apartments.Infrastructure.Utilities.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Apartments.Infrastructure.Utilities.Extensions;

public static class UtilityDbModelExtensions
{
    public static UtilityMeasurement ToApartment(this UtilityMeasurementDbModel dbModel)
    {
        var apartment = new UtilityMeasurement
        {
            ApartmentId  = dbModel.ApartmentId,
            MeasurementDate = dbModel.MeasurementDate,
            UtilityType = (UtilityType)dbModel.UtilityType,
            UtilityUsage = dbModel.UtilityUsage
        };

        return apartment;
    }

    public static UtilityMeasurementDbModel FromDomainModel(UtilityMeasurement domainModel) =>
        new()
        {
            ApartmentId = domainModel.ApartmentId,
            MeasurementDate = domainModel.MeasurementDate,
            UtilityType = (int)domainModel.UtilityType,
            UtilityUsage = domainModel.UtilityUsage
        };
}

public static class UtilityModuleExtensions
{
    public static IServiceCollection AddUtilityInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IUtilityRepository, UtilityRepository>();

        return services;
    }
}
