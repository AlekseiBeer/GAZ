using GasHimApi.API.Dtos;
using GasHimApi.API.Services;
using GasHimApi.API.Utils;
using GasHimApi.Data;
using GasHimApi.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace GasHimApi.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProcessesController : ControllerBase
{
    private readonly IRepository<Process> _repo;
    private readonly IProcessesReadService _read;

    public ProcessesController(IRepository<Process> repo, IProcessesReadService read)
    {
        _repo = repo;
        _read = read;
    }

    // -------- READ (paged) ----------
    // GET /api/processes/paged?search=&take=50&cursor=
    [HttpGet("paged")]
    public async Task<ActionResult<PagedResult<ProcessDto>>> GetPaged(
        [FromQuery] string? search,
        [FromQuery] int take = 50,
        [FromQuery] string? cursor = null,
        CancellationToken ct = default)
    {
        var result = await _read.GetPageAsync(new ProcessQuery(search, take, cursor), ct);
        return Ok(result);
    }

    // -------- CRUD (DTO) ----------
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProcessDto>>> GetAll()
    {
        var entities = await _repo.GetAllAsync();
        var dtos = entities.Select(p => p.ToDto());
        return Ok(dtos);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProcessDto>> Get(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null) return NotFound();
        return Ok(entity.ToDto());
    }

    [HttpPost]
    public async Task<ActionResult<ProcessDto>> Create([FromBody] ProcessDto dto)
    {
        var entity = new Process();
        entity.UpdateEntity(dto);
        await _repo.AddAsync(entity);
        return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity.ToDto());
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ProcessDto dto)
    {
        if (id != dto.Id) return BadRequest("ID mismatch");
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null) return NotFound();
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