using GasHimApi.Data;
using GasHimApi.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GasHimApi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcessesController : ControllerBase
    {
        private readonly IRepository<Process> _processesRepository;

        public ProcessesController(IRepository<Process> processesRepository)
        {
            _processesRepository = processesRepository;
        }

        // GET: api/processes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Process>>> GetAll()
        {
            var processes = await _processesRepository.GetAllAsync();
            return Ok(processes);
        }

        // GET: api/processes/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Process>> Get(int id)
        {
            var process = await _processesRepository.GetByIdAsync(id);
            if (process == null)
            {
                return NotFound();
            }
            return Ok(process);
        }

        // Новый эндпоинт: GET: api/processes/byname/{name}
        // Возвращает процесс по его названию (без учета регистра)
        [HttpGet("byname/{name}")]
        public async Task<ActionResult<Process>> GetByName(string name)
        {
            var processes = await _processesRepository.GetAllAsync();
            var process = processes.FirstOrDefault(p => p.Name!.Equals(name, System.StringComparison.OrdinalIgnoreCase));
            if (process == null)
            {
                return NotFound();
            }
            return Ok(process);
        }

        // POST: api/processes
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Process process)
        {
            await _processesRepository.AddAsync(process);
            return CreatedAtAction(nameof(Get), new { id = process.Id }, process);
        }

        // PUT: api/processes/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Process process)
        {
            if (id != process.Id)
            {
                return BadRequest();
            }
            await _processesRepository.UpdateAsync(process);
            return NoContent();
        }

        // DELETE: api/processes/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _processesRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
