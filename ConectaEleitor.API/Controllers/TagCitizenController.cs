using ConectaEleitor.Application.DTOs.TagCitizensDTOs;
using ConectaEleitor.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConectaEleitor.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TagCitizenController : ControllerBase
{
    private readonly ITagCitizenService _tagCitizenService;

    public TagCitizenController(ITagCitizenService tagCitizenService)
    {
        _tagCitizenService = tagCitizenService;
    }

    [HttpPost]
    public async Task<IActionResult> AddTagToCitizen([FromBody] TagCitizenCreateDTO dto)
    {
        return Ok(await _tagCitizenService.AddTagToCitizen(dto));
    }

    [HttpGet("citizen/{citizenId}")]
    public async Task<IActionResult> GetAllByCitizenId(Guid citizenId)
    {
        return Ok(await _tagCitizenService.GetAllByCitizenId(citizenId));
    }

    [HttpGet("tag/{tagId}")]
    public async Task<IActionResult> GetAllByTagId(Guid tagId)
    {
        return Ok(await _tagCitizenService.GetAllByTagId(tagId));
    }

    [HttpDelete("{tagId}/{citizenId}")]
    public async Task<IActionResult> RemoveTagFromCitizen(Guid tagId, Guid citizenId)
    {
        await _tagCitizenService.RemoveTagFromCitizen(tagId, citizenId);

        return Ok("TAG removida do eleitor com sucesso!");
    }
}