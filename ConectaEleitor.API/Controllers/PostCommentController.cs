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
    [Authorize]
    public class PostCommentController : ControllerBase
    {
        private readonly IPostCommentService _postCommentService;

        public PostCommentController(IPostCommentService postCommentService)
        {
            _postCommentService = postCommentService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AssemblymanPostCommentCreateDTO commentData)
        {
            return Ok(await _postCommentService.CreatePostComment(commentData));
        }

        [HttpGet("{postId}/comments")]
        public async Task<IActionResult> GetAllComments([FromQuery] PaginationParams paginationParams, Guid postId)
        {
            return Ok(await _postCommentService.GetAllCommentsByPostId(paginationParams, postId));
        }

        [HttpGet("{commentId}")]
        public async Task<IActionResult> GetComment(Guid commentId)
        {
            return Ok(await _postCommentService.GetById(commentId));
        }

        [HttpDelete("{commentId}")]
        public async Task<IActionResult> Delete(Guid commentId)
        {
            return Ok(await _postCommentService.Delete(commentId));
        }
    }
}
