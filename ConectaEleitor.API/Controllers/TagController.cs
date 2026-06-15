using ConectaEleitor.Application.DTOs.Pagination;
using ConectaEleitor.Application.DTOs.TagsDTOs;
using ConectaEleitor.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConectaEleitor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TagController : ControllerBase
    {
        private readonly ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TagCreateDTO dados)
        {
            return Ok(await _tagService.Create(dados));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParams paginationParams)
        {
            return Ok(await _tagService.GetAll(paginationParams));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            return Ok(await _tagService.GetById(id));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] TagUpdateDTO dados)
        {
            return Ok(await _tagService.Update(dados, id));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _tagService.Delete(id);
            return Ok("TAG deletada com sucesso!");
        }
    }
}
