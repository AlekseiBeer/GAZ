using System.Threading;
using System.Threading.Tasks;
using GasHimApi.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace GasHimApi.API.Controllers.Tests
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestDataController : ControllerBase
    {
        private readonly ITestDataService _testDataService;
        private readonly IHostEnvironment _env;

        public TestDataController(ITestDataService testDataService, IHostEnvironment env)
        {
            _testDataService = testDataService;
            _env = env;
        }

        [HttpDelete("all")]
        public async Task<IActionResult> ClearAll(CancellationToken ct)
        {
            if (!_env.IsDevelopment())
                return Forbid("Доступ запрещён вне Development.");

            await _testDataService.ClearAllAsync(ct);
            return NoContent();
        }

        [HttpPost("seed")]
        public async Task<IActionResult> Seed([FromQuery] int count = 1000, CancellationToken ct = default)
        {
            if (!_env.IsDevelopment())
                return Forbid("Доступ запрещён вне Development.");

            await _testDataService.SeedSubstancesAsync(count, ct);
            return Ok(new { inserted = count });
        }

        // ----------- НОВЫЕ ЭНДПОИНТЫ ДЛЯ ПРОЦЕССОВ -----------

        [HttpDelete("processes")]
        public async Task<IActionResult> ClearProcesses(CancellationToken ct)
        {
            if (!_env.IsDevelopment())
                return Forbid("Доступ запрещён вне Development.");

            await _testDataService.ClearProcessesAsync(ct);
            return NoContent();
        }

        [HttpPost("processes/seed")]
        public async Task<IActionResult> SeedProcesses([FromQuery] int count = 300, CancellationToken ct = default)
        {
            if (!_env.IsDevelopment())
                return Forbid("Доступ запрещён вне Development.");

            await _testDataService.SeedProcessesAsync(count, ct);
            return Ok(new { inserted = count });
        }
    }
}
