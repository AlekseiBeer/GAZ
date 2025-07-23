using Microsoft.EntityFrameworkCore;
using GasHimApi.Data.Models;

namespace GasHimApi.Data
{
    public class ChemicalDbContext : DbContext
    {
        public ChemicalDbContext(DbContextOptions<ChemicalDbContext> options)
            : base(options)
        { }

        public DbSet<Substance> Substances { get; set; }
        public DbSet<Process> Processes { get; set; }
    }
}