using ConectaEleitor.Application.DTOs.FilesDTOs;

namespace ConectaEleitor.Application.Interfaces;

public interface IPostFileStorageService
{
    Task<PostFileUploadResultDTO> UploadPostFileAsync(
        Stream inputStream,
        string originalFileName,
        string contentType,
        long length,
        Guid ownerId,
        Guid postId,
        CancellationToken cancellationToken = default);

    Task DeletePostFileAsync(
        string fileUrl,
        Guid ownerId,
        CancellationToken cancellationToken = default);
}