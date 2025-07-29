using GasHimApi.Contracts.Substances;

namespace GasHimApi.Services.Services.Substances;

/// <summary>
/// Только мутационные операции: создать, обновить, удалить для веществ
/// </summary>
public interface ISubstancesCommandService
{
    Task<SubstanceDto> AddAsync(SubstanceCreateDto createDto, CancellationToken ct);
    Task<bool> UpdateAsync(int id, SubstanceUpdateDto updateDto, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
}
