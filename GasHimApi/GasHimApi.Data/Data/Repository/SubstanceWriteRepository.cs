using GasHimApi.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace GasHimApi.Data.Data.Repository;

public class SubstanceWriteRepository : IWriteRepository<Substance>
{
    private readonly ChemicalDbContext _context;

    public SubstanceWriteRepository(ChemicalDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Substance entity, CancellationToken ct)
    {
        await _context.Substances.AddAsync(entity, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Substance entity, CancellationToken ct)
    {
        _context.Substances.Update(entity);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Substance entity, CancellationToken ct)
    {
        _context.Substances.Remove(entity);
        await _context.SaveChangesAsync(ct);
    }

    public async Task SaveChangesAsync(CancellationToken ct)
    {
        await _context.SaveChangesAsync(ct);
    }
}
