using Apartments.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Apartments.Infrastructure.EntityFramework.Contexts;

public class IdentityDbContext(DbContextOptions<IdentityDbContext> options) 
    : IdentityDbContext<User>(options);
