using Microsoft.EntityFrameworkCore;
using GasHimApi.Data.Models;

namespace GasHimApi.Data.Data.Repository
{
    public class ProcessWriteRepository : IWriteRepository<Process>
    {
        private readonly ChemicalDbContext _context;
        public ProcessWriteRepository(ChemicalDbContext context) => _context = context;

        public async Task AddAsync(Process process, CancellationToken ct)
        {
            await _context.Processes.AddAsync(process, ct);
            await _context.SaveChangesAsync(ct);
        }

        public async Task UpdateAsync(Process process, CancellationToken ct)
        {
            _context.Processes.Update(process);
            await _context.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Process process, CancellationToken ct)
        {
            _context.Processes.Remove(process);
            await _context.SaveChangesAsync(ct);
        }

        public Task SaveChangesAsync(CancellationToken ct) =>
            _context.SaveChangesAsync(ct);
    }
}
