using GasHimApi.API.Dtos;
using GasHimApi.API.Services;
using GasHimApi.API.Utils;
using GasHimApi.Data;
using GasHimApi.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace GasHimApi.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SubstancesController : ControllerBase
{
    private readonly IRepository<Substance> _repo;
    private readonly ISubstancesReadService _readService;

    public SubstancesController(IRepository<Substance> repo,
                                ISubstancesReadService readService)
    {
        _repo = repo;
        _readService = readService;
    }

    // ---------- PAGED ----------
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
        var result = await _readService.GetPageAsync(query, ct);
        return Ok(result);
    }

    // ---------- CRUD ----------

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SubstanceDto>>> GetAll()
    {
        var all = await _repo.GetAllAsync();
        return Ok(all.Select(x => x.ToDto()));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<SubstanceDto>> Get(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return NotFound();
        return Ok(entity.ToDto());
    }

    [HttpPost]
    public async Task<ActionResult<SubstanceDto>> Post([FromBody] SubstanceCreateUpdateDto dto)
    {
        var entity = dto.ToEntity();
        await _repo.AddAsync(entity);
        return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity.ToDto());
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody] SubstanceCreateUpdateDto dto)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return NotFound();

        entity.UpdateEntity(dto);
        await _repo.UpdateAsync(entity);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _repo.DeleteAsync(id);
        return NoContent();
    }
}
