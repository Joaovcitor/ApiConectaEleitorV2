using ConectaEleitor.Domain.Enums;

namespace ConectaEleitor.Application.DTOs.CitizenDTOs;

public class CitizenUpdateDTO
{
    public string FullName { get; set; } = string.Empty;
    public string? Nickname { get; set; }

    public string? Cpf { get; set; }
    public string? VoterRegistration { get; set; }

    public DateOnly? BirthDate { get; set; }

    public string? Phone { get; set; }
    public string? WhatsApp { get; set; }

    public string? ZipCode { get; set; }
    public string? Street { get; set; }
    public string? Number { get; set; }
    public string? Complement { get; set; }
    public string? Neighborhood { get; set; }
    public string? District { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }

    public string? ElectoralZone { get; set; }
    public string? ElectoralSection { get; set; }

    public CitizenType Type { get; set; }

    public Guid? LeaderId { get; set; }

    public Guid? UserId { get; set; }

    public bool IsActive { get; set; }
}