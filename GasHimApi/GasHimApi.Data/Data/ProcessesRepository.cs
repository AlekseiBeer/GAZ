using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GasHimApi.Data.Models;

namespace GasHimApi.Data
{
    public class ProcessesRepository : IRepository<Process>
    {
        private readonly ChemicalDbContext _context;

        public ProcessesRepository(ChemicalDbContext context)
        {
            _context = context;
        }

        public async Task<List<Process>> GetAllAsync()
            => await _context.Processes.AsNoTracking().ToListAsync();

        public async Task<Process?> GetByIdAsync(int id)
            => await _context.Processes.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

        public async Task AddAsync(Process process)
        {
            await _context.Processes.AddAsync(process);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Process process)
        {
            var exists = await _context.Processes.AnyAsync(p => p.Id == process.Id);
            if (!exists) throw new InvalidOperationException($"Process {process.Id} not found");

            _context.Processes.Update(process);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var process = await _context.Processes.FindAsync(id);
            if (process == null) return;

            _context.Processes.Remove(process);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Process>> SearchAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query)) return new List<Process>();

            return await _context.Processes
                .Where(p =>
                    (p.MainInputs != null && p.MainInputs.Contains(query)) ||
                    (p.AdditionalInputs != null && p.AdditionalInputs.Contains(query)) ||
                    (p.MainOutputs != null && p.MainOutputs.Contains(query)) ||
                    (p.AdditionalOutputs != null && p.AdditionalOutputs.Contains(query)))
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
