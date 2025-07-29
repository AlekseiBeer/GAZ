using GasHimApi.Contracts;
using GasHimApi.Contracts.Substances;

namespace GasHimApi.Services.Services.Substances;

/// <summary>
/// “олько операции чтени€: постранична€ выдача, получение списка и по-ID дл€ веществ
/// </summary>
public interface ISubstancesQueryService
{
    Task<PagedResult<SubstanceDto>> GetPageAsync(SubstanceQuery query, CancellationToken ct);
    Task<List<SubstanceDto>> GetAllAsync(CancellationToken ct);
    Task<SubstanceDto?> GetByIdAsync(int id, CancellationToken ct);
    Task<List<SubstanceDto>> SearchAsync(string search, CancellationToken ct);
}
