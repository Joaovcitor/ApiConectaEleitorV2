using ConectaEleitor.Application.DTOs.FilesDTOs;
using ConectaEleitor.Application.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace ConectaEleitor.Infrastructure.Services;

public class PostFileStorageService : IPostFileStorageService
{
    private readonly IWebHostEnvironment _environment;

    private const long MaxFileSize = 5 * 1024 * 1024;

    private static readonly Dictionary<string, string[]> AllowedExtensions = new()
    {
        [".jpg"] = ["image/jpeg"],
        [".jpeg"] = ["image/jpeg"],
        [".png"] = ["image/png"],
        [".webp"] = ["image/webp"],
        [".pdf"] = ["application/pdf"]
    };

    public PostFileStorageService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<PostFileUploadResultDTO> UploadPostFileAsync(
        Stream inputStream,
        string originalFileName,
        string contentType,
        long length,
        Guid ownerId,
        Guid postId,
        CancellationToken cancellationToken = default)
    {
        if (inputStream is null || length == 0)
            throw new InvalidOperationException("Arquivo inválido.");

        if (length > MaxFileSize)
            throw new InvalidOperationException("Arquivo excede o tamanho máximo permitido.");

        originalFileName = Path.GetFileName(originalFileName);
        var extension = Path.GetExtension(originalFileName).ToLowerInvariant();

        if (!AllowedExtensions.ContainsKey(extension))
            throw new InvalidOperationException("Extensão de arquivo não permitida.");

        if (!AllowedExtensions[extension].Contains(contentType))
            throw new InvalidOperationException("Tipo de arquivo inválido.");

        await ValidateFileSignatureAsync(inputStream, extension, cancellationToken);

        var safeFileName = $"{Guid.NewGuid()}{extension}";
        var postDirectory = GetPostDirectory(ownerId, postId);

        Directory.CreateDirectory(postDirectory);

        var filePath = Path.Combine(postDirectory, safeFileName);

        await using var outputStream = new FileStream(filePath, FileMode.CreateNew);
        await inputStream.CopyToAsync(outputStream, cancellationToken);

        return new PostFileUploadResultDTO
        {
            FileName = safeFileName,
            OriginalFileName = originalFileName,
            FileUrl = $"/uploads/posts/{ownerId}/{postId}/{safeFileName}",
            ContentType = contentType,
            SizeInBytes = length
        };
    }

    public Task DeletePostFileAsync(
        string fileUrl,
        Guid ownerId,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(fileUrl))
            throw new InvalidOperationException("Arquivo inválido.");

        var relativePath = fileUrl.TrimStart('/');

        if (!relativePath.StartsWith($"uploads/posts/{ownerId}/"))
            throw new UnauthorizedAccessException("Você não pode remover esse arquivo.");

        var webRootPath = GetWebRootPath();

        var fullPath = Path.GetFullPath(Path.Combine(webRootPath, relativePath));
        var allowedRoot = Path.GetFullPath(
            Path.Combine(webRootPath, "uploads", "posts", ownerId.ToString()));

        if (!fullPath.StartsWith(allowedRoot))
            throw new UnauthorizedAccessException("Caminho de arquivo inválido.");

        if (File.Exists(fullPath))
            File.Delete(fullPath);

        return Task.CompletedTask;
    }

    private string GetPostDirectory(Guid ownerId, Guid postId)
    {
        return Path.Combine(
            GetWebRootPath(),
            "uploads",
            "posts",
            ownerId.ToString(),
            postId.ToString());
    }

    private string GetWebRootPath()
    {
        if (!string.IsNullOrWhiteSpace(_environment.WebRootPath))
            return _environment.WebRootPath;

        return Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
    }

    private static async Task ValidateFileSignatureAsync(
        Stream stream,
        string extension,
        CancellationToken cancellationToken)
    {
        var signatures = new Dictionary<string, List<byte[]>>
        {
            [".jpg"] = [[0xFF, 0xD8, 0xFF]],
            [".jpeg"] = [[0xFF, 0xD8, 0xFF]],
            [".png"] = [[0x89, 0x50, 0x4E, 0x47]],
            [".webp"] = [[0x52, 0x49, 0x46, 0x46]],
            [".pdf"] = [[0x25, 0x50, 0x44, 0x46]]
        };

        if (!signatures.TryGetValue(extension, out var allowedSignatures))
            throw new InvalidOperationException("Assinatura de arquivo inválida.");

        var maxSignatureLength = allowedSignatures.Max(s => s.Length);
        var buffer = new byte[maxSignatureLength];

        var bytesRead = await stream.ReadAsync(buffer, cancellationToken);

        if (stream.CanSeek)
            stream.Position = 0;

        var isValid = allowedSignatures.Any(signature =>
            bytesRead >= signature.Length &&
            signature.SequenceEqual(buffer.Take(signature.Length)));

        if (!isValid)
            throw new InvalidOperationException("O conteúdo do arquivo não corresponde ao tipo informado.");
    }
}