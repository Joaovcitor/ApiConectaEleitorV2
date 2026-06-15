using ConectaEleitor.Application.DTOs.TagCitizensDTOs;

namespace ConectaEleitor.Application.Interfaces;

public interface ITagCitizenService
{
    Task<TagCitizenResponseDTO> AddTagToCitizen(TagCitizenCreateDTO dto);

    Task<IEnumerable<TagCitizenResponseDTO>> GetAllByCitizenId(Guid citizenId);

    Task<IEnumerable<TagCitizenResponseDTO>> GetAllByTagId(Guid tagId);

    Task RemoveTagFromCitizen(Guid tagId, Guid citizenId);
}