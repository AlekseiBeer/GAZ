using AutoMapper;
using GasHimApi.Contracts.Processes;
using GasHimApi.Data.Data.Repository;
using GasHimApi.Data.Models;

namespace GasHimApi.API.Services.Processes;

public class ProcessCommandService : IProcessCommandService
{
    private readonly IWriteRepository<Process> _writeRepo;
    private readonly IReadRepository<Process> _readRepo;
    private readonly IMapper _mapper;

    public ProcessCommandService(IWriteRepository<Process> writeRepo, IReadRepository<Process> readRepo, IMapper mapper)
    {
        _writeRepo = writeRepo;
        _readRepo = readRepo;
        _mapper = mapper;
    }

    public async Task<ProcessDto> AddAsync(ProcessCreateDto dtoCreate, CancellationToken ct)
    {
        var entity = _mapper.Map<Process>(dtoCreate);
        await _writeRepo.AddAsync(entity, ct);
        return _mapper.Map<ProcessDto>(entity);
    }

    public async Task<bool> UpdateAsync(int id, ProcessCreateDto dtoCreate, CancellationToken ct)
    {
        var entity = await _readRepo.GetByIdAsync(id, ct);
        if (entity == null)
            return false;

        _mapper.Map(dtoCreate, entity);

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