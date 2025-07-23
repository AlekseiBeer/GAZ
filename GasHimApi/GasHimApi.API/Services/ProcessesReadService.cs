using GasHimApi.API.Dtos;
using GasHimApi.API.Utils;
using GasHimApi.Data;
using GasHimApi.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace GasHimApi.API.Services;

public class ProcessesReadService : IProcessesReadService
{
    private readonly ChemicalDbContext _db;

    public ProcessesReadService(ChemicalDbContext db)
    {
        _db = db;
    }

    public async Task<PagedResult<ProcessDto>> GetPageAsync(ProcessQuery query, CancellationToken ct)
    {
        var take = query.Take <= 0 ? 50 : query.Take;
        var q = _db.Processes.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            // ILIKE для PostgreSQL
            var pattern = $"%{query.Search.Trim()}%";
            q = q.Where(p =>
                EF.Functions.ILike(p.Name!, pattern) ||
                EF.Functions.ILike(p.MainInputs!, pattern) ||
                EF.Functions.ILike(p.MainOutputs!, pattern) ||
                EF.Functions.ILike(p.AdditionalInputs!, pattern) ||
                EF.Functions.ILike(p.AdditionalOutputs!, pattern));
        }

        // Курсор
        if (!string.IsNullOrEmpty(query.Cursor))
        {
            var (name, id) = CursorHelper.Decode(query.Cursor);
            // те же правила сортировки
            q = q.Where(p => string.Compare(p.Name, name, StringComparison.Ordinal) > 0
                             || (p.Name == name && p.Id > id));
        }

        // Сортировка стабильная
        q = q.OrderBy(p => p.Name).ThenBy(p => p.Id);

        var items = await q.Take(take + 1).ToListAsync(ct);

        var hasMore = items.Count > take;
        if (hasMore) items.RemoveAt(items.Count - 1);

        var dtos = items.Select(p => p.ToDto()).ToList();

        string? nextCursor = null;
        if (hasMore && dtos.Count > 0)
        {
            var last = items[^1];
            nextCursor = CursorHelper.Encode(last.Name!, last.Id);
        }

        // Total можешь не считать – ставь null
        return new PagedResult<ProcessDto>(dtos, null, nextCursor, hasMore);
    }
}
