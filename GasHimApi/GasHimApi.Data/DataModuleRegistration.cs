using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GasHimApi.Data.Models;

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
            services.AddScoped<IRepository<Substance>, SubstancesRepository>();
            services.AddScoped<IRepository<Process>, ProcessesRepository>();

            return services;
        }
    }
}