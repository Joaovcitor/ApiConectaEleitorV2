using ConectaEleitor.Application.DTOs.CitizenDTOs;
using ConectaEleitor.Application.DTOs.DemandsDTOs;
using ConectaEleitor.Application.DTOs.Pagination;
using ConectaEleitor.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConectaEleitor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DemandController : ControllerBase
    {
        private readonly IDemandService _demandService;

        public DemandController(IDemandService demandService)
        {
            _demandService = demandService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DemandCreateDTO dados)
        {
            return Ok(await _demandService.Create(dados));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParams paginationParams)
        {
            return Ok(await _demandService.GetAll(paginationParams));
        }

        [HttpGet("{demandId}")]
        public async Task<IActionResult> GetById(Guid demandId)
        {
            return Ok(await _demandService.GetById(demandId));
        }

        [HttpGet("demands/citizen/{citizenId}")]
        public async Task<IActionResult> GetByCitizenId(Guid citizenId, [FromQuery] PaginationParams paginationParams)
        {
            return Ok(await _demandService.GetAllDemandsByCitizenId(paginationParams, citizenId));
        }

        [HttpPut("demand/update/{demandId}")]
        public async Task<IActionResult> Update(Guid demandId, [FromBody] DemandUpdateDTO dados)
        {
            return Ok(await _demandService.Update(dados, demandId));
        }

        [HttpDelete("demand/delete/{demandId}")]
        public async Task<IActionResult> Delete(Guid demandId)
        {
            await _demandService.Delete(demandId);
            return NoContent();
        }
    }
}
