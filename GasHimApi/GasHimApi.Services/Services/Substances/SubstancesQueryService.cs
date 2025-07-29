using AutoMapper;
using GasHimApi.Contracts;
using GasHimApi.Contracts.Substances;
using GasHimApi.Data;
using GasHimApi.Data.Data.Repository;
using GasHimApi.Data.Models;
using Microsoft.EntityFrameworkCore;
using GasHimApi.Contracts.Utils;

namespace GasHimApi.Services.Services.Substances;

public class SubstancesQueryService : ISubstancesQueryService
{
    private readonly ChemicalDbContext _db;
    private readonly IReadRepository<Substance> _readRepo;
    private readonly IMapper _mapper;

    public SubstancesQueryService(ChemicalDbContext db, IReadRepository<Substance> readRepo, IMapper mapper)
    {
        _db = db;
        _readRepo = readRepo;
        _mapper = mapper;
    }

    public async Task<PagedResult<SubstanceDto>> GetPageAsync(SubstanceQuery query, CancellationToken ct)
    {
        int take = query.Take is > 0 and <= 200 ? query.Take : 50;
        var q = _db.Substances.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var s = $"%{query.Search.Trim()}%";
            q = q.Where(x =>
                EF.Functions.ILike(x.Name!, s) ||
                (x.Synonyms != null && EF.Functions.ILike(x.Synonyms, s))
            );
        }

        q = q.OrderBy(x => x.Name).ThenBy(x => x.Id);

        if (!string.IsNullOrEmpty(query.Cursor))
        {
            var (name, id) = CursorHelper.Decode(query.Cursor);
            q = string.IsNullOrEmpty(name)
                ? q.Where(x => x.Id > id)
                : q.Where(x => x.Name == name && x.Id > id || string.Compare(x.Name, name) > 0);
        }

        var list = await q.Take(take + 1).ToListAsync(ct);
        bool hasMore = list.Count > take;
        string? nextCursor = null;
        if (hasMore)
        {
            var last = list[take];
            nextCursor = CursorHelper.Encode(last.Name!, last.Id);
            list.RemoveAt(take);
        }

        var dtos = list.Select(x => _mapper.Map<SubstanceDto>(x)).ToList();
        return new PagedResult<SubstanceDto>(dtos, null, nextCursor, hasMore);
    }

    public async Task<List<SubstanceDto>> GetAllAsync(CancellationToken ct)
    {
        var all = await _readRepo.GetAllAsync(ct);
        return all.Select(x => _mapper.Map<SubstanceDto>(x)).ToList();
    }

    public async Task<SubstanceDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        var e = await _readRepo.GetByIdAsync(id, ct);
        return e is null ? null : _mapper.Map<SubstanceDto>(e);
    }

    public async Task<List<SubstanceDto>> SearchAsync(string search, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(search))
            return new List<SubstanceDto>();
        var q = _readRepo.Query();
        var pattern = search.Trim();
        q = q.Where(s =>
            (s.Name != null && s.Name.Contains(pattern)) ||
            (s.Synonyms != null && s.Synonyms.Contains(pattern))
        );
        var entities = await q.ToListAsync(ct);
        return entities.Select(_mapper.Map<SubstanceDto>).ToList();
    }
}
