using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace OAuth2.Persistence
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<OAuth2DbContext>
    {
        public OAuth2DbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

            var optionsBuilder = new DbContextOptionsBuilder<OAuth2DbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("OAuth2ConnectString"));

            return new OAuth2DbContext(optionsBuilder.Options);
        }
    }
}
