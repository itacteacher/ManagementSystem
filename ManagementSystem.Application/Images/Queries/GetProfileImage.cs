using Ardalis.GuardClauses;
using ManagementSystem.Application.Common.Interfaces;
using ManagementSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ManagementSystem.Application.Images.Queries;

public record GetProfileImageQuery (Guid UserId) : IRequest<FileContentResult>;

public class GetProfileImageQueryHandler : IRequestHandler<GetProfileImageQuery, FileContentResult>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStorageService _fileStorageService;

    public GetProfileImageQueryHandler (IApplicationDbContext context, IFileStorageService fileStorageService)
    {
        _context = context;
        _fileStorageService = fileStorageService;
    }

    public async Task<FileContentResult> Handle (GetProfileImageQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FindAsync(request.UserId);
        Guard.Against.NotFound(nameof(User), user);

        var imagePath = user.ProfileImagePath;
        Guard.Against.NotFound("image", imagePath);

        var fileBytes = await _fileStorageService.GetFileAsync(imagePath, cancellationToken);

        var fileExtension = Path.GetExtension(imagePath).ToLowerInvariant();
        string contentType = fileExtension switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".bmp" => "image/bmp",
            _ => "application/octet-stream"
        };

        return new FileContentResult(fileBytes, contentType);
    }
}