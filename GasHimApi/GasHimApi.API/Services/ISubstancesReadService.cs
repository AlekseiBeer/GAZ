using System.Threading;
using System.Threading.Tasks;
using GasHimApi.API.Dtos;

namespace GasHimApi.API.Services
{
    public interface ISubstancesReadService
    {
        Task<PagedResult<SubstanceDto>> GetPageAsync(SubstanceQuery q, CancellationToken ct);
    }
}

