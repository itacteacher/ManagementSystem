using Microsoft.AspNetCore.Http;

namespace ManagementSystem.Application.Common.Interfaces;

public interface IFileStorageService
{
    Task<string> SaveFileAsync (IFormFile formFile, CancellationToken cancellationToken);

    Task<byte[]> GetFileAsync (string relativePath, CancellationToken cancellationToken);

    void DeleteFile (string relativePath);
}
