using GasHimApi.Contracts;
using GasHimApi.Contracts.Processes;
using GasHimApi.API.Services.Processes;
using Microsoft.AspNetCore.Mvc;

namespace GasHimApi.API.Controllers.Processes;

[Route("api/processes")]
[ApiController]
public class ProcessesReadController : ControllerBase
{
    private readonly IProcessQueryService _processQueryService;

    public ProcessesReadController(IProcessQueryService processesService) =>
        _processQueryService = processesService ?? throw new ArgumentNullException(nameof(processesService));

    // GET /api/processes/paged?search=&take=50&cursor=
    [HttpGet("paged")]
    public async Task<ActionResult<PagedResult<ProcessDto>>> GetPaged(
        [FromQuery] string? search,
        [FromQuery] int take = 50,
        [FromQuery] string? cursor = null,
        CancellationToken ct = default)
    {
        var result = await _processQueryService.GetPageAsync(new ProcessQuery(search, take, cursor), ct);
        return Ok(result);
    }

    // GET /api/processes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProcessDto>>> GetAll(CancellationToken ct)
    {
        var dtos = await _processQueryService.GetAllAsync(ct);
        return Ok(dtos);
    }

    // GET /api/processes/{id}
    [HttpGet("{id:int}", Name = "GetProcessById")]
    public async Task<ActionResult<ProcessDto>> GetById(int id, CancellationToken ct)
    {
        var dto = await _processQueryService.GetByIdAsync(id, ct);
        if (dto is null) 
            return NotFound();
        return Ok(dto);
    }

    // GET /api/processes/search?term=acetone
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<ProcessDto>>> Search(
        [FromQuery(Name = "term")] string term,
        CancellationToken ct)
    {
        var results = await _processQueryService.SearchAsync(term, ct);
        return Ok(results);
    }
}