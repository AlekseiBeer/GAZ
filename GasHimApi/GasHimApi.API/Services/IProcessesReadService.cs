using GasHimApi.API.Dtos;
using System.Diagnostics;

namespace GasHimApi.API.Services;

public interface IProcessesReadService
{
    Task<PagedResult<ProcessDto>> GetPageAsync(ProcessQuery query, CancellationToken ct);

    //Task<List<ProcessDto>> GetAllAsync(CancellationToken ct);
}