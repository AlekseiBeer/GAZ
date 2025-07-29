using GasHimApi.Contracts;
using GasHimApi.Contracts.Processes;

namespace GasHimApi.API.Services.Processes
{
    /// <summary>
    /// Только операции чтения: постраничная выдача, получение списка и по-ID
    /// </summary>
    public interface IProcessQueryService
    {
        Task<PagedResult<ProcessDto>> GetPageAsync(ProcessQuery query, CancellationToken ct);

        Task<List<ProcessDto>> GetAllAsync(CancellationToken ct);

        /// <returns>null, если не найден</returns>
        Task<ProcessDto?> GetByIdAsync(int id, CancellationToken ct);

        Task<List<ProcessDto>> SearchAsync(string search, CancellationToken ct);
    }
}
