using ConectaEleitor.Application.DTOs.CitizenDTOs;
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
    public class CitizenController : ControllerBase
    {
        private readonly ICitizenService _citizenService;

        public CitizenController(ICitizenService citizenService)
        {
            _citizenService = citizenService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CitizenCreateDTO dados)
        {
            return Ok(await _citizenService.Create(dados));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParams paginationParams)
        {
            return Ok(await _citizenService.GetAll(paginationParams));
        }
        
        [HttpGet("citizens/leaders")]
        public async Task<IActionResult> GetAllLeaders([FromQuery] PaginationParams paginationParams)
        {
            return Ok(await _citizenService.GetPagedCitizensLeaderAsync(paginationParams));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok(await _citizenService.GetById(id));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] CitizenUpdateDTO dados)
        {
            return Ok(await _citizenService.Update(dados, id));
        }
    }
}
