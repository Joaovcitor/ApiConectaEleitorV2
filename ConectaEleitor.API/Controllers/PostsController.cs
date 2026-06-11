using ConectaEleitor.Application.DTOs.AssemblymanPostsDTOs;
using ConectaEleitor.Application.DTOs.Pagination;
using ConectaEleitor.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConectaEleitor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] AssemblymanPostCreateDTO dados)
        {
            return Ok(await _postService.Create(dados));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllPosts([FromQuery] PaginationParams paginationParams)
        {
            return Ok(await _postService.GetAllPosts(paginationParams));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok(await _postService.GetById(id));
        }

        [HttpPut("post/update/{id}")]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] AssemblymanPostUpdateDTO dados, Guid id)
        {
            return Ok(await _postService.Update(dados, id));
        }
        [HttpDelete("post/delete/{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _postService.Delete(id));
        }
    }
}
