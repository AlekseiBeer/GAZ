using GasHimApi.Contracts.Substances;

namespace GasHimApi.Services.Services.Substances;

/// <summary>
/// ������ ����������� ��������: �������, ��������, ������� ��� �������
/// </summary>
public interface ISubstancesCommandService
{
    Task<SubstanceDto> AddAsync(SubstanceCreateDto createDto, CancellationToken ct);
    Task<bool> UpdateAsync(int id, SubstanceUpdateDto updateDto, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
}
