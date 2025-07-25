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
        var take = (query.Take <= 0 || query.Take > 200) ? 50 : query.Take;
        var q = _db.Processes.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var pattern = $"%{query.Search.Trim()}%";
            q = q.Where(p =>
                EF.Functions.ILike(p.Name!, pattern) ||
                EF.Functions.ILike(p.MainInputs!, pattern) ||
                EF.Functions.ILike(p.MainOutputs!, pattern) ||
                EF.Functions.ILike(p.AdditionalInputs!, pattern) ||
                EF.Functions.ILike(p.AdditionalOutputs!, pattern));
        }

        q = q.OrderBy(p => p.Name).ThenBy(p => p.Id);

        // Фильтрация по курсору на уровне запроса
        if (!string.IsNullOrEmpty(query.Cursor))
        {
            var (name, id) = CursorHelper.Decode(query.Cursor);
            if (!string.IsNullOrEmpty(name))
            {
                // Для EF Core: сравнение по Name и Id
                q = q.Where(p => (p.Name == name && p.Id > id) || string.Compare(p.Name, name) > 0);
            }
        }

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

        // Если остался только один элемент (последний), возвращаем его
        // (логика уже корректна, но добавим явный комментарий)

        var dtos = list.Select(p => p.ToDto()).ToList();
        return new PagedResult<ProcessDto>(dtos, null, nextCursor, hasMore);
    }
}
