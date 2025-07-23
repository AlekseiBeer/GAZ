using GasHimApi.API.Dtos;
using GasHimApi.API.Utils;
using GasHimApi.Data;
using GasHimApi.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace GasHimApi.API.Services;

public class SubstancesReadService : ISubstancesReadService
{
    private readonly ChemicalDbContext _db;

    public SubstancesReadService(ChemicalDbContext db)
    {
        _db = db;
    }

    public async Task<PagedResult<SubstanceDto>> GetPageAsync(SubstanceQuery query, CancellationToken ct)
    {
        IQueryable<Substance> q = _db.Substances.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.Search) && query.Search.Length >= 3)
        {
            var s = query.Search.Trim();
            q = q.Where(x =>
                (x.Name != null && EF.Functions.ILike(x.Name, $"%{s}%")) ||
                (x.Synonyms != null && EF.Functions.ILike(x.Synonyms, $"%{s}%")));
        }

        if (!string.IsNullOrWhiteSpace(query.Cursor))
        {
            var (lastName, lastId) = CursorHelper.Decode(query.Cursor);
            q = q.Where(x =>
                string.Compare(x.Name, lastName, StringComparison.Ordinal) > 0 ||
                (x.Name == lastName && x.Id > lastId));
        }

        q = q.OrderBy(x => x.Name).ThenBy(x => x.Id);

        var take = query.Take is <= 0 or > 200 ? 50 : query.Take;
        var list = await q.Take(take + 1).ToListAsync(ct);

        bool hasMore = list.Count > take;
        string? nextCursor = null;
        if (hasMore)
        {
            var last = list[take];
            nextCursor = CursorHelper.Encode(last.Name ?? string.Empty, last.Id);
            list.RemoveAt(take);
        }

        int? total = null; // если захочешь, можешь посчитать await q.CountAsync(ct)

        var items = list.Select(x => new SubstanceDto(x.Id, x.Name ?? string.Empty, x.Synonyms))
                        .ToList();

        return new PagedResult<SubstanceDto>(items, total, nextCursor, hasMore);
    }
}