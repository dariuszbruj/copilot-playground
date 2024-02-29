using Apartments.Infrastructure.Apartments.Models;
using Microsoft.EntityFrameworkCore;

namespace Apartments.Infrastructure.EntityFramework.Contexts;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options)
{
    public DbSet<ApartmentDbModel> Apartments { get; set; } = default!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ApartmentDbModelConfiguration());
        modelBuilder.ApplyConfiguration(new ApartmentAddressDbModelConfiguration());
    }
}
