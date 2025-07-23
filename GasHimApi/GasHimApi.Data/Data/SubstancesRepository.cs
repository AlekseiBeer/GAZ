using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GasHimApi.Data.Models;

namespace GasHimApi.Data
{
    public class SubstancesRepository : IRepository<Substance>
    {
        private readonly ChemicalDbContext _context;

        public SubstancesRepository(ChemicalDbContext context)
        {
            _context = context;
        }

        public async Task<List<Substance>> GetAllAsync()
            => await _context.Substances.AsNoTracking().ToListAsync();

        public async Task<Substance?> GetByIdAsync(int id)
            => await _context.Substances.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);

        public async Task AddAsync(Substance substance)
        {
            await _context.Substances.AddAsync(substance);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Substance substance)
        {
            var exists = await _context.Substances.AnyAsync(s => s.Id == substance.Id);
            if (!exists) throw new InvalidOperationException($"Substance {substance.Id} not found");

            _context.Substances.Update(substance);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var substance = await _context.Substances.FindAsync(id);
            if (substance == null) return;

            _context.Substances.Remove(substance);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Substance>> SearchAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query)) return new List<Substance>();

            return await _context.Substances
                .Where(s => EF.Functions.ILike(s.Name!, $"%{query}%") ||
                           (s.Synonyms != null && EF.Functions.ILike(s.Synonyms, $"%{query}%")))
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
