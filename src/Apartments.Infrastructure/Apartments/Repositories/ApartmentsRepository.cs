using Apartments.Application.Modules.Apartments;
using Apartments.Application.Modules.Apartments.Results;
using Apartments.Domain.Models;
using Apartments.Infrastructure.Apartments.Extensions;
using Apartments.Infrastructure.EntityFramework.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Apartments.Infrastructure.Apartments.Repositories;

public class ApartmentRepository(ApplicationDbContext context)
    : IApartmentRepository
{
    private readonly ApplicationDbContext _context = context 
        ?? throw new ArgumentNullException(nameof(context));

    public async Task<Apartment> GetByIdAsync(Guid id,
        CancellationToken cancellationToken = default)
    {
        var apartmentDbModel = await _context.Apartments
            .Include(a => a.Address)
            .FirstAsync(x => x.Guid == id, cancellationToken: cancellationToken);

        return apartmentDbModel.ToApartment();
    }
        
    public async Task<IEnumerable<Apartment>> GetApartmentsAsync(CancellationToken cancellationToken = default)
    {
        var apartmentDbModel = await _context.Apartments
            .Include(a => a.Address)
            .ToListAsync( cancellationToken: cancellationToken);

        return apartmentDbModel.Select(x => x.ToApartment()).ToList();
    }

    public async Task<Guid> AddAsync(Apartment apartment, 
        CancellationToken cancellationToken = default)
    {
        var dbModel = ApartmentsDbModelExtensions.FromDomainModel(apartment);
        _context.Apartments.Add(dbModel);
            
        await _context.SaveChangesAsync(cancellationToken);

        return dbModel.Guid;
    }

    public async Task<ApartmentResult> UpdateAsync(Apartment apartment, 
        CancellationToken cancellationToken = default)
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
        await _context.SaveChangesAsync(cancellationToken);

        return ApartmentResult.Ok();
    }

    public async Task DeleteAsync(Guid id, 
        CancellationToken cancellationToken = default)
    {
        var apartmentDbModel = await _context.Apartments
            .FirstOrDefaultAsync(x => x.Guid == id, cancellationToken: cancellationToken);

        if (apartmentDbModel is not null)
        {
            _context.Apartments.Remove(apartmentDbModel);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
