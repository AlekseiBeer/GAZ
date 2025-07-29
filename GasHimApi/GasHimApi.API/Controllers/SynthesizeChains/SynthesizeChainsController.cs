using GasHimApi.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GasHimApi.API.Controllers.SynthesizeChains
{
    [ApiController]
    [Route("api/chains")]
    public class SynthesizeChainsController : ControllerBase
    {
        private readonly ChainsService _chainsService;

        public SynthesizeChainsController(ChainsService chainsService)
        {
            _chainsService = chainsService;
        }

        // POST /api/chains
        // Тело запроса:
        // {
        //   "startSubstance": "Пропилен",
        //   "targetSubstance": "Акриловая кислота"
        // }
        [HttpPost]
        public async Task<ActionResult<List<List<string>>>> Post([FromBody] ChainSearchRequest request, CancellationToken ct)
        {
            if (!string.IsNullOrWhiteSpace(request.StartSubstance) && string.IsNullOrWhiteSpace(request.TargetSubstance))
            {
                // Если задано только стартовое вещество – используем DFSFromStart
                var chains = await _chainsService.GetChainsByStartOnly(request.StartSubstance, ct);
                return Ok(chains);
            }
            else if (string.IsNullOrWhiteSpace(request.StartSubstance) && !string.IsNullOrWhiteSpace(request.TargetSubstance))
            {
                // Если задано только конечное вещество – используем ReverseDFSForTarget
                var chains = await _chainsService.GetChainsByTargetOnly(request.TargetSubstance, ct);
                return Ok(chains);
            }
            else if (!string.IsNullOrWhiteSpace(request.StartSubstance) && !string.IsNullOrWhiteSpace(request.TargetSubstance))
            {
                // Если заданы оба вещества – используем DFS (полный поиск цепочки)
                var chains = await _chainsService.GetChainsByStartAndTarget(request.StartSubstance, request.TargetSubstance, ct);
                return Ok(chains);
            }
            else
            {
                return BadRequest("Нужно указать хотя бы одно вещество: стартовое или конечное.");
            }
        }

        // GET /api/chains/all – комплексный поиск всех цепочек по всем веществам
        [HttpGet("all")]
        public async Task<ActionResult<List<List<string>>>> GetAllChains(CancellationToken ct)
        {
            var chains = await _chainsService.GetAllChains(ct);
            return Ok(chains);
        }
    }

    public class ChainSearchRequest
    {
        public string? StartSubstance { get; set; }
        public string? TargetSubstance { get; set; }
    }
}
