using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApplication1.API;
using WebApplication1.Infrastructure.MySqlRepositories;

namespace api.IntegrationTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        // Use a unique database name per factory instance to ensure test class isolation
        private readonly string _databaseName = $"InMemoryDbForTesting_{Guid.NewGuid()}";

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove the real database context registration
                var descriptor = services.SingleOrDefault(d => 
                    d.ServiceType == typeof(DbContextOptions<MySqlDbContext>));
                
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Add in-memory database for testing
                // Use the same database name for all requests within this factory instance
                services.AddDbContext<MySqlDbContext>(options =>
                {
                    options.UseInMemoryDatabase(_databaseName);
                });

                // Build the service provider
                var sp = services.BuildServiceProvider();

                // Create a scope to get scoped services
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<MySqlDbContext>();

                    // Ensure the database is created
                    db.Database.EnsureCreated();

                    // Seed the database with test data if needed
                    // SeedData.Initialize(db);
                }
            });

            builder.UseEnvironment("Testing");
        }
    }
}
