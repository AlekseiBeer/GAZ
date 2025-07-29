using GasHimApi.Contracts;
using GasHimApi.Contracts.Substances;
using GasHimApi.Services.Services.Substances;
using Microsoft.AspNetCore.Mvc;

namespace GasHimApi.API.Controllers.Substances;

[Route("api/substances")]
[ApiController]
public class SubstancesReadController : ControllerBase
{
    private readonly ISubstancesQueryService _queryService;

    public SubstancesReadController(ISubstancesQueryService queryService)
    {
        _queryService = queryService;
    }

    // GET /api/substances/paged?search=...&take=50&cursor=...
    [HttpGet("paged")]
    public async Task<ActionResult<PagedResult<SubstanceDto>>> GetPaged(
        [FromQuery] string? search,
        [FromQuery] int take = 50,
        [FromQuery] string? cursor = null,
        CancellationToken ct = default)
    {
        var safeTake = take is <= 0 or > 200 ? 50 : take;
        var query = new SubstanceQuery(search, safeTake, cursor);
        var result = await _queryService.GetPageAsync(query, ct);
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SubstanceDto>>> GetAll(CancellationToken ct)
    {
        var all = await _queryService.GetAllAsync(ct);
        return Ok(all);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<SubstanceDto>> Get(int id, CancellationToken ct)
    {
        var entity = await _queryService.GetByIdAsync(id, ct);
        if (entity == null) return NotFound();
        return Ok(entity);
    }

    // GET /api/substances/search?term=acetone
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<SubstanceDto>>> Search(
        [FromQuery(Name = "term")] string term,
        CancellationToken ct)
    {
        var results = await _queryService.SearchAsync(term, ct);
        return Ok(results);
    }
}
