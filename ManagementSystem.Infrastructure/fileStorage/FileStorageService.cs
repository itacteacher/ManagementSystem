using ManagementSystem.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ManagementSystem.Infrastructure.fileStorage;
public class FileStorageService : IFileStorageService
{
    private readonly ILogger<FileStorageService> _logger;
    private readonly string _basePath;
    private readonly long _maxFileSize;
    private readonly string[] _allowedExtensions;

    public FileStorageService (IConfiguration configuration, ILogger<FileStorageService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        _basePath = configuration["FileStorage:BasePath"]
            ?? throw new InvalidOperationException("BasePath not configured");

        var maxFileSizeStr = configuration["FileStorage:MaxFileSize"];

        _maxFileSize = long.TryParse(maxFileSizeStr, out var maxFileSize)
            ? maxFileSize
            : 5 * 1024 * 1024;

        var allowedExtensionsStr = configuration["FileStorage:AllowedExtensions"];
        _allowedExtensions = !string.IsNullOrEmpty(allowedExtensionsStr)
            ? allowedExtensionsStr.Split(',').Select(ext => ext.Trim()).ToArray()
            : [".jpg", ".jpeg", ".png", ".gif"];
    }

    public void DeleteFile (string relativePath)
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), relativePath);

        if (File.Exists(filePath))
        {
            try
            {
                File.Delete(filePath);
                _logger.LogInformation("");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new InvalidOperationException();
            }
        }
        else
        {
            _logger.LogInformation("");
        }
    }

    public async Task<byte[]> GetFileAsync (string relativePath, CancellationToken cancellationToken)
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), relativePath);

        if (!File.Exists(filePath))
        {
            _logger.LogWarning("");
            throw new FileNotFoundException();
        }

        try
        {
            return await File.ReadAllBytesAsync(filePath, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw new InvalidOperationException();
        }
    }

    public async Task<string> SaveFileAsync (IFormFile formFile, CancellationToken cancellationToken)
    {
        if (formFile == null || formFile.Length == 0)
        {
            _logger.LogWarning("");
            throw new ArgumentException();
        }

        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), _basePath);
        Directory.CreateDirectory(uploadsFolder);

        var uniqueName = $"{Guid.NewGuid()}_{formFile.FileName}";
        var filePath = Path.Combine(uploadsFolder, uniqueName);

        try
        {
            if (formFile.Length > _maxFileSize)
            {
                _logger.LogWarning("");
                throw new ArgumentException();
            }

            var fileExtension = Path.GetExtension(formFile.FileName)?.ToLowerInvariant();

            if (!IsValidFileExtension(fileExtension))
            {
                _logger.LogWarning("");
                throw new ArgumentException();
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await formFile.CopyToAsync(stream, cancellationToken);
            }

            return Path.Combine(_basePath, uniqueName).Replace("\\", "/");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error occured! {ex.Message}");
            throw new InvalidOperationException();
        }
    }

    private bool IsValidFileExtension (string extension)
    {
        return !string.IsNullOrEmpty(extension) && _allowedExtensions.Contains(extension.ToLowerInvariant());
    }
}
