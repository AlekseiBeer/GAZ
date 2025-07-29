using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GasHimApi.Data.Models;
using GasHimApi.Data.Data.Repository;

namespace GasHimApi.Data
{
    public static class DataModuleRegistration
    {
        public static IServiceCollection AddDataModuleServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            }

            // Регистрируем DbContext с подключением к PostgreSQL
            services.AddDbContext<ChemicalDbContext>(options =>
                options.UseNpgsql(connectionString));

            // Регистрируем репозитории
            services.AddScoped<IReadRepository<Substance>, SubstanceReadRepository>();
            services.AddScoped<IWriteRepository<Substance>, SubstanceWriteRepository>();
            services.AddScoped<IWriteRepository<Process>, ProcessWriteRepository>();
            services.AddScoped<IReadRepository<Process>, ProcessReadRepository>();

            return services;
        }
    }
}