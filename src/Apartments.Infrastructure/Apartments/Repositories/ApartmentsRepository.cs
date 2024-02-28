using Apartments.Domain;
using Apartments.Domain.Services.Apartments;
using Apartments.Domain.Services.Apartments.Results;
using Apartments.Infrastructure.Apartments.Extensions;
using Apartments.Infrastructure.EntityFramework.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Apartments.Infrastructure.Apartments.Repositories
{
    public class ApartmentRepository : IApartmentRepository
    {
        private readonly ApplicationDbContext _context;

        public ApartmentRepository(ApplicationDbContext context)
        {
            _context = context 
                ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Apartment> GetApartmentByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var apartmentDbModel = await _context.Apartments
                .FirstAsync(x => x.Guid == id, cancellationToken);

            return apartmentDbModel.ToApartment();
        }

        public async Task AddAsync(Apartment apartment, CancellationToken cancellationToken = default)
        {
            var dbModel = ApartmentsDbModelExtensions.FromDomainModel(apartment);
            _context.Apartments.Add(dbModel);
            
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<ApartmentResult> UpdateAsync(Apartment apartment, CancellationToken cancellationToken = default)
        {
            var apartmentDbModel = await _context.Apartments
                .FirstOrDefaultAsync(x => x.Guid == apartment.Id, cancellationToken: cancellationToken);

            if (apartmentDbModel is null)
            {
                return ApartmentResult.NotFound();
            }
            
            var dbModel = ApartmentsDbModelExtensions.FromDomainModel(apartment);
            dbModel.Name = apartment.Name;
            
            _context.Apartments.Update(apartmentDbModel);
            await _context.SaveChangesAsync();

            return ApartmentResult.Ok();
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var apartmentDbModel = await _context.Apartments.FirstOrDefaultAsync(x => x.Guid == id);

            if (apartmentDbModel is not null)
            {
                _context.Apartments.Remove(apartmentDbModel);
                await _context.SaveChangesAsync();
            }
        }
    }
}