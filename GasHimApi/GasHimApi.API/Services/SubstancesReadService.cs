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

        q = q.OrderBy(x => x.Name).ThenBy(x => x.Id);

        // Фильтрация по курсору на уровне запроса
        if (!string.IsNullOrWhiteSpace(query.Cursor))
        {
            var (lastName, lastId) = CursorHelper.Decode(query.Cursor);
            if (!string.IsNullOrEmpty(lastName))
            {
                q = q.Where(x => (x.Name == lastName && x.Id > lastId) || string.Compare(x.Name, lastName) > 0);
            }
        }

        var take = (query.Take <= 0 || query.Take > 200) ? 50 : query.Take;
        var list = await q.Take(take + 1).ToListAsync(ct);

        bool hasMore = list.Count > take;
        string? nextCursor = null;
        if (hasMore)
        {
            var last = list[take];
            if (!string.IsNullOrEmpty(last.Name))
                nextCursor = CursorHelper.Encode(last.Name, last.Id);
            list.RemoveAt(take);
        }

        var items = list.Select(x => new SubstanceDto(x.Id, x.Name ?? string.Empty, x.Synonyms)).ToList();
        return new PagedResult<SubstanceDto>(items, null, nextCursor, hasMore);
    }
}