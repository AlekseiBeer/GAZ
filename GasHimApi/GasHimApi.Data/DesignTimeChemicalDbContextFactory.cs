using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace GasHimApi.Data;

public class DesignTimeChemicalDbContextFactory : IDesignTimeDbContextFactory<ChemicalDbContext>
{
    public ChemicalDbContext CreateDbContext(string[] args)
    {
        var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "GasHimApi.API");

        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        var optionsBuilder = new DbContextOptionsBuilder<ChemicalDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new ChemicalDbContext(optionsBuilder.Options);
    }
}
