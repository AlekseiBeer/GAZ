using AutoMapper;
using GasHimApi.Contracts;
using GasHimApi.Contracts.Processes;
using GasHimApi.Contracts.Utils;
using GasHimApi.Data;
using GasHimApi.Data.Data.Repository;
using GasHimApi.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace GasHimApi.API.Services.Processes
{
    public class ProcessQueryService : IProcessQueryService
    {
        private readonly ChemicalDbContext _db;
        private readonly IReadRepository<Process> _readRepo;
        private readonly IMapper _mapper;

        public ProcessQueryService(ChemicalDbContext db, IReadRepository<Process> readRepo, IMapper mapper)
        {
            _db = db;
            _readRepo = readRepo;
            _mapper = mapper;
        }

        public async Task<PagedResult<ProcessDto>> GetPageAsync(ProcessQuery query, CancellationToken ct)
        {
            int take = query.Take is > 0 and <= 200 ? query.Take : 50;
            var q = _db.Processes.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                var p = $"%{query.Search.Trim()}%";
                q = q.Where(x =>
                    EF.Functions.ILike(x.Name!, p) ||
                    EF.Functions.ILike(x.PrimaryFeedstocks!, p) ||
                    EF.Functions.ILike(x.SecondaryFeedstocks!, p) ||
                    EF.Functions.ILike(x.PrimaryProducts!, p) ||
                    EF.Functions.ILike(x.ByProducts!, p)
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

            var dtos = list.Select(x => _mapper.Map<ProcessDto>(x)).ToList();
            return new PagedResult<ProcessDto>(dtos, null, nextCursor, hasMore);
        }

        public async Task<List<ProcessDto>> GetAllAsync(CancellationToken ct)
        {
            var all = await _readRepo.GetAllAsync(ct);
            return all.Select(x => _mapper.Map<ProcessDto>(x)).ToList();
        }

        public async Task<ProcessDto?> GetByIdAsync(int id, CancellationToken ct)
        {
            var e = await _readRepo.GetByIdAsync(id, ct);
            return e is null ? null : _mapper.Map<ProcessDto>(e);
        }

        public async Task<List<ProcessDto>> SearchAsync(string search, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(search))
                return new List<ProcessDto>();

            // строим IQueryable из репозитория
            var q = _readRepo.Query();

            var pattern = search.Trim();
            q = q.Where(p =>
                (p.PrimaryFeedstocks != null && p.PrimaryFeedstocks.Contains(pattern)) ||
                (p.SecondaryFeedstocks != null && p.SecondaryFeedstocks.Contains(pattern)) ||
                (p.PrimaryProducts != null && p.PrimaryProducts.Contains(pattern)) ||
                (p.ByProducts != null && p.ByProducts.Contains(pattern))
            );

            var entities = await q.ToListAsync(ct);
            return entities.Select(_mapper.Map<ProcessDto>).ToList();
        }
    }
}
