using System.Collections.Generic;
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
        {
            return await _context.Substances.ToListAsync();
        }

        public async Task<Substance> GetByIdAsync(int id)
        {
            return await _context.Substances.FindAsync(id);
        }

        public async Task AddAsync(Substance substance)
        {
            await _context.Substances.AddAsync(substance);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Substance substance)
        {
            _context.Entry(substance).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var substance = await GetByIdAsync(id);
            if (substance != null)
            {
                _context.Substances.Remove(substance);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Substance>> SearchAsync(string query)
        {
            return await _context.Substances
                .Where(s => s.Name.Contains(query) || s.Synonyms.Contains(query))
                .ToListAsync();
        }
    }
}
