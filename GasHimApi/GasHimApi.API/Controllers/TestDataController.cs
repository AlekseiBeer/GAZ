using System.Threading;
using System.Threading.Tasks;
using GasHimApi.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace GasHimApi.API.Controllers
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

        /// <summary>
        /// Удалить все данные из таблиц (TRUNCATE + RESTART IDENTITY).
        /// Разрешено только в Development.
        /// </summary>
        [HttpDelete("all")]
        public async Task<IActionResult> ClearAll(CancellationToken ct)
        {
            if (!_env.IsDevelopment())
                return Forbid("Доступ запрещён вне Development.");

            await _testDataService.ClearAllAsync(ct);
            return NoContent();
        }

        /// <summary>
        /// Засидить N тестовых веществ (по умолчанию 1000).
        /// Разрешено только в Development.
        /// </summary>
        [HttpPost("seed")]
        public async Task<IActionResult> Seed([FromQuery] int count = 1000, CancellationToken ct = default)
        {
            if (!_env.IsDevelopment())
                return Forbid("Доступ запрещён вне Development.");

            await _testDataService.SeedSubstancesAsync(count, ct);
            return Ok(new { inserted = count });
        }
    }
}
