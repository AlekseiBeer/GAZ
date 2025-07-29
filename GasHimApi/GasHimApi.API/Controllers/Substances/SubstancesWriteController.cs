using GasHimApi.Contracts.Substances;
using GasHimApi.Services.Services.Substances;
using Microsoft.AspNetCore.Mvc;

namespace GasHimApi.API.Controllers.Substances;

[Route("api/substances")]
[ApiController]
public class SubstancesWriteController : ControllerBase
{
    private readonly ISubstancesCommandService _commandService;

    public SubstancesWriteController(ISubstancesCommandService commandService)
    {
        _commandService = commandService;
    }

    [HttpPost]
    public async Task<ActionResult<SubstanceDto>> Post([FromBody] SubstanceCreateDto dto, CancellationToken ct)
    {
        var created = await _commandService.AddAsync(dto, ct);
        return CreatedAtAction("Get", new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody] SubstanceUpdateDto dto, CancellationToken ct)
    {
        var updated = await _commandService.UpdateAsync(id, dto, ct);
        if (!updated) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var deleted = await _commandService.DeleteAsync(id, ct);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
