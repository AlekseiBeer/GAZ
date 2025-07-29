using GasHimApi.Contracts.Processes;

namespace GasHimApi.API.Services.Processes;

/// <summary>
/// Только мутационные операции: создать, обновить, удалить
/// </summary>
public interface IProcessCommandService
{
    Task<ProcessDto> AddAsync(ProcessCreateDto createDto, CancellationToken ct);

    Task<bool> UpdateAsync(int id, ProcessCreateDto createDto, CancellationToken ct);

    Task<bool> DeleteAsync(int id, CancellationToken ct);
}