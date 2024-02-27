using Apartments.Domain;
using Apartments.Domain.Services.Apartments;
using Apartments.Domain.Services.Apartments.Results;
using Apartments.Infrastructure.Apartments.Extensions;
using Apartments.Infrastructure.EntityFramework.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Apartments.Infrastructure.Apartments.Repositories
{
    public class ApartmentRepository(ApplicationDbContext context) 
        : IApartmentRepository
    {
        public async Task<Apartment> GetApartmentByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var apartmentDbModel = await context.Apartments
                .FirstAsync(x => x.Guid == id, cancellationToken);

            return apartmentDbModel.ToApartment();
        }

        public async Task AddAsync(Apartment apartment, CancellationToken cancellationToken = default)
        {
            var dbModel = ApartmentsDbModelExtensions.FromDomainModel(apartment);
            context.Apartments.Add(dbModel);
            
            await context.SaveChangesAsync();
        }

        public async Task<ApartmentResult> UpdateAsync(Apartment apartment, CancellationToken cancellationToken = default)
        {
            var apartmentDbModel = await context.Apartments.FirstOrDefaultAsync(x => x.Guid == apartment.Id);

            if (apartmentDbModel is null)
            {
                return ApartmentResult.NotFound();
            }
            
            var dbModel = ApartmentsDbModelExtensions.FromDomainModel(apartment);
            dbModel.Name = apartment.Name;
            
            context.Apartments.Update(apartmentDbModel);
            await context.SaveChangesAsync();

            return ApartmentResult.Ok();
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var apartmentDbModel = await context.Apartments.FirstOrDefaultAsync(x => x.Guid == id);

            if (apartmentDbModel is not null)
            {
                context.Apartments.Remove(apartmentDbModel);
                await context.SaveChangesAsync();
            }
        }
    }
}
