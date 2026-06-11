namespace ConectaEleitor.Application.DTOs.FilesDTOs;

public class PostFileUploadResultDTO
{
    public string FileName { get; set; } = null!;
    public string OriginalFileName { get; set; } = null!;
    public string FileUrl { get; set; } = null!;
    public string ContentType { get; set; } = null!;
    public long SizeInBytes { get; set; }
}