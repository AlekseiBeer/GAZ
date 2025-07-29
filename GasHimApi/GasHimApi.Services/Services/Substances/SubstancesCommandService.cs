using AutoMapper;
using GasHimApi.Contracts.Substances;
using GasHimApi.Data.Data.Repository;
using GasHimApi.Data.Models;

namespace GasHimApi.Services.Services.Substances;

public class SubstancesCommandService : ISubstancesCommandService
{
    private readonly IWriteRepository<Substance> _writeRepo;
    private readonly IReadRepository<Substance> _readRepo;
    private readonly IMapper _mapper;

    public SubstancesCommandService(IWriteRepository<Substance> writeRepo, IReadRepository<Substance> readRepo, IMapper mapper)
    {
        _writeRepo = writeRepo;
        _readRepo = readRepo;
        _mapper = mapper;
    }

    public async Task<SubstanceDto> AddAsync(SubstanceCreateDto createDto, CancellationToken ct)
    {
        var entity = _mapper.Map<Substance>(createDto);
        await _writeRepo.AddAsync(entity, ct);
        return _mapper.Map<SubstanceDto>(entity);
    }

    public async Task<bool> UpdateAsync(int id, SubstanceUpdateDto updateDto, CancellationToken ct)
    {
        var entity = await _readRepo.GetByIdAsync(id, ct);
        if (entity == null)
            return false;
        _mapper.Map(updateDto, entity);
        await _writeRepo.UpdateAsync(entity, ct);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var entity = await _readRepo.GetByIdAsync(id, ct);
        if (entity == null)
            return false;
        await _writeRepo.DeleteAsync(entity, ct);
        return true;
    }
}
