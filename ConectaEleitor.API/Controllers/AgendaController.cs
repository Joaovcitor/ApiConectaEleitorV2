using ConectaEleitor.Application.DTOs.AgendasDTOs;
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
    public class AgendaController : ControllerBase
    {
        private readonly IAgendaService _agendaService;

        public AgendaController(IAgendaService agendaService)
        {
            _agendaService = agendaService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AgendaCreateDTO dados)
        {
            return Ok(await _agendaService.CreateAsync(dados));
        }

        [HttpGet("agenda/my")]
        public async Task<IActionResult> GetMyAgendas([FromQuery] PaginationParams paginationParams)
        {
            return Ok(await _agendaService.GetAll(paginationParams));
        }

        [HttpGet("agenda/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok(await _agendaService.GetByIdAsync(id));
        }

        [HttpPut("agenda/{id}")]
        public async Task<IActionResult> Update([FromBody] AgendaUpdateDTO dados, Guid id)
        {
            return Ok(await _agendaService.UpdateAsync(id, dados));
        }

        [HttpDelete("agenda/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _agendaService.DeleteAsync(id);
            return Ok("Agenda deletada com sucesso!");
        }
    }
}
