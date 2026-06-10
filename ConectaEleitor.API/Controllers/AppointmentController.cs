using ConectaEleitor.Application.DTOs.AppointmentsDTOs;
using ConectaEleitor.Application.DTOs.Pagination;
using ConectaEleitor.Application.Interfaces;
using ConectaEleitor.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConectaEleitor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AppointmentCreateDTO appointment)
        {
            return Ok(await _appointmentService.Create(appointment));
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationParams paginationParams)
        {
            return Ok(await _appointmentService.GetAll(paginationParams));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok(await _appointmentService.GetById(id));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] AppointmentsUpdateDTO dados)
        {
            return Ok(await _appointmentService.Update(id, dados));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _appointmentService.Delete(id));
        }
    }
}
