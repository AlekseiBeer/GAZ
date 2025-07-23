using GasHimApi.API.Dtos;

namespace GasHimApi.API.Services;

public interface IProcessesReadService
{
    Task<PagedResult<ProcessDto>> GetPageAsync(ProcessQuery query, CancellationToken ct);
}