using Ardalis.GuardClauses;
using ManagementSystem.Application.Common.Interfaces;
using ManagementSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace ManagementSystem.Application.Images.Commands;

public record UploadProfileImageCommand (Guid UserId, IFormFile Image) : IRequest<Unit>;

public class UploadProfileImageHandler : IRequestHandler<UploadProfileImageCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStorageService _fileStorageService;

    public UploadProfileImageHandler (IApplicationDbContext context, IFileStorageService fileStorageService)
    {
        _context = context;
        _fileStorageService = fileStorageService;
    }

    public async Task<Unit> Handle (UploadProfileImageCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FindAsync(request.UserId, cancellationToken);

        Guard.Against.NotFound(nameof(User), request.UserId);

        var relativePath = await _fileStorageService.SaveFileAsync(request.Image, cancellationToken);

        user!.ProfileImagePath = relativePath;
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}