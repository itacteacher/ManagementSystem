using Ardalis.GuardClauses;
using ManagementSystem.Application.Common.Interfaces;
using ManagementSystem.Domain.Entities;
using MediatR;

namespace ManagementSystem.Application.Images.Commands;

public record DeleteProfileImageCommand (Guid UserId) : IRequest<Unit>;

public class DeleteProfileImageCommandHandler : IRequestHandler<DeleteProfileImageCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStorageService _fileStorageService;

    public DeleteProfileImageCommandHandler (IApplicationDbContext context, IFileStorageService fileStorageService)
    {
        _context = context;
        _fileStorageService = fileStorageService;
    }

    public async Task<Unit> Handle (DeleteProfileImageCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FindAsync(request.UserId, cancellationToken);

        Guard.Against.NotFound(nameof(User), request.UserId);

        if (!string.IsNullOrEmpty(user!.ProfileImagePath))
        {
            _fileStorageService.DeleteFile(user.ProfileImagePath);
            user.ProfileImagePath = null;
            await _context.SaveChangesAsync(cancellationToken);
        }

        return Unit.Value;
    }
}
