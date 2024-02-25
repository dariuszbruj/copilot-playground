using Apartments.Infrastructure.EntityFramework;
using Apartments.Infrastructure.EntityFramework.Contexts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Apartment.Api.UnitTests.Infrastructure.EntityFramework
{
    public class EntityFrameworkServiceExtensionsTests
    {
        
        [Fact]
        public void AddInfrastructure_ShouldAddDbContexts()
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?> { { "ConnectionStrings:DefaultConnection", "TestConnectionString" } }) 
                .Build();

            // Act
            services.AddInfrastructure(configuration);

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            Assert.NotNull(serviceProvider.GetService<IdentityDbContext>());
            Assert.NotNull(serviceProvider.GetService<ApplicationDbContext>());
        }
    }
}
