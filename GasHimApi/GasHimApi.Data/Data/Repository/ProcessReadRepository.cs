using Microsoft.EntityFrameworkCore;
using GasHimApi.Data.Models;

namespace GasHimApi.Data.Data.Repository
{
    public class ProcessReadRepository: IReadRepository<Process>
    {
        private readonly ChemicalDbContext _context;
        public ProcessReadRepository(ChemicalDbContext context) => _context = context;

        public IQueryable<Process> Query() =>
            _context.Processes.AsNoTracking();

        public async Task<List<Process>> GetAllAsync(CancellationToken ct) =>
            await Query().ToListAsync(ct);

        public async Task<Process?> GetByIdAsync(int id, CancellationToken ct) =>
            await _context.Processes.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id, ct);
    }
}