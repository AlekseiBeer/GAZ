using GasHimApi.Data;
using GasHimApi.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace GasHimApi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubstancesController : ControllerBase
    {
        private readonly IRepository<Substance> _substancesRepository;

        public SubstancesController(IRepository<Substance> substancesRepository)
        {
            _substancesRepository = substancesRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Substance>>> GetAll()
        {
            var substances = await _substancesRepository.GetAllAsync();
            return Ok(substances);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Substance>> Get(int id)
        {
            var substance = await _substancesRepository.GetByIdAsync(id);
            if (substance == null)
            {
                return NotFound();
            }
            return Ok(substance);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Substance substance)
        {
            await _substancesRepository.AddAsync(substance);
            return CreatedAtAction(nameof(Get), new { id = substance.Id }, substance);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Substance substance)
        {
            if (id != substance.Id)
            {
                return BadRequest();
            }
            await _substancesRepository.UpdateAsync(substance);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _substancesRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
