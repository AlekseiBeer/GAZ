using GasHimApi.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace GasHimApi.Data.Data.Repository;

public class SubstanceReadRepository : IReadRepository<Substance>
{
    private readonly ChemicalDbContext _context;

    public SubstanceReadRepository(ChemicalDbContext context)
    {
        _context = context;
    }

    public async Task<List<Substance>> GetAllAsync(CancellationToken ct)
        => await _context.Substances.AsNoTracking().ToListAsync(ct);

    public async Task<Substance?> GetByIdAsync(int id, CancellationToken ct)
        => await _context.Substances.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id, ct);

    public IQueryable<Substance> Query() => _context.Substances.AsNoTracking();
}
