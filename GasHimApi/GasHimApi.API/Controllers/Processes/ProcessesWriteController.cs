using GasHimApi.Contracts.Processes;
using GasHimApi.API.Services.Processes;
using Microsoft.AspNetCore.Mvc;

namespace GasHimApi.API.Controllers.Processes;

[Route("api/processes")]
[ApiController]
public class ProcessesWriteController: ControllerBase
{
    private readonly IProcessCommandService _processService;

    public ProcessesWriteController(IProcessCommandService processesService) =>
        _processService = processesService ?? throw new ArgumentNullException(nameof(processesService));

    // POST /api/processes
    [HttpPost]
    public async Task<ActionResult<ProcessDto>> Create([FromBody] ProcessCreateDto createDto, CancellationToken ct)
    {
        var created = await _processService.AddAsync(createDto, ct);
        return CreatedAtRoute(
            routeName: "GetProcessById",
            routeValues: new { id = created.Id },
            value: created
        );
    }

    // PUT /api/processes/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ProcessCreateDto updateDto, CancellationToken ct)
    {
        bool updated = await _processService.UpdateAsync(id, updateDto, ct);
        if (!updated)
            return NotFound(ct);
        return NoContent();
    }

    // DELETE /api/processes/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        bool deleted = await _processService.DeleteAsync(id, ct);
        if (!deleted)
            return NotFound();
        return NoContent();
    }
}