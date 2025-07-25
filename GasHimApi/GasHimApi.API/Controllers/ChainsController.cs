﻿using GasHimApi.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GasHimApi.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChainsController : ControllerBase
    {
        private readonly ChainsService _chainsService;

        public ChainsController(ChainsService chainsService)
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
        public async Task<ActionResult<List<List<string>>>> Post([FromBody] ChainSearchRequest request)
        {
            if (!string.IsNullOrWhiteSpace(request.StartSubstance) && string.IsNullOrWhiteSpace(request.TargetSubstance))
            {
                // Если задано только стартовое вещество – используем DFSFromStart
                var chains = await _chainsService.GetChainsByStartOnly(request.StartSubstance);
                return Ok(chains);
            }
            else if (string.IsNullOrWhiteSpace(request.StartSubstance) && !string.IsNullOrWhiteSpace(request.TargetSubstance))
            {
                // Если задано только конечное вещество – используем ReverseDFSForTarget
                var chains = await _chainsService.GetChainsByTargetOnly(request.TargetSubstance);
                return Ok(chains);
            }
            else if (!string.IsNullOrWhiteSpace(request.StartSubstance) && !string.IsNullOrWhiteSpace(request.TargetSubstance))
            {
                // Если заданы оба вещества – используем DFS (полный поиск цепочки)
                var chains = await _chainsService.GetChainsByStartAndTarget(request.StartSubstance, request.TargetSubstance);
                return Ok(chains);
            }
            else
            {
                return BadRequest("Нужно указать хотя бы одно вещество: стартовое или конечное.");
            }
        }

        // GET /api/chains/all – комплексный поиск всех цепочек по всем веществам
        [HttpGet("all")]
        public async Task<ActionResult<List<List<string>>>> GetAllChains()
        {
            var chains = await _chainsService.GetAllChains();
            return Ok(chains);
        }
    }

    public class ChainSearchRequest
    {
        public string? StartSubstance { get; set; }
        public string? TargetSubstance { get; set; }
    }
}
