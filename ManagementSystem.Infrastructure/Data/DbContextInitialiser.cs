using ManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ManagementSystem.Infrastructure.Data;

public class DbContextInitialiser
{
    private readonly ApplicationDbContext _context;

    public DbContextInitialiser (ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task InitialiseAsync ()
    {
        try
        {
            await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task SeedAsync ()
    {
        try
        {
            var adminId = Guid.NewGuid();

            if (!_context.Users.Any())
            {
                _context.Users.AddRange(
                    new User
                    {
                        Id = adminId,
                        Username = "admin",
                        Email = "admin@example.com",
                        FirstName = "Administrator",
                        LastName = "User",
                        CreatedBy = null,
                        CreatedAt = DateTime.Now,
                        LastModifiedBy = null,
                        LastModifiedAt = DateTime.Now
                    },
                    new User
                    {
                        Id = Guid.NewGuid(),
                        Username = "jdoe",
                        Email = "jdoe@example.com",
                        FirstName = "John",
                        LastName = "Doe",
                        CreatedBy = adminId,
                        CreatedAt = DateTime.Now,
                        LastModifiedBy = adminId,
                        LastModifiedAt = DateTime.Now
                    },
                    new User
                    {
                        Id = Guid.NewGuid(),
                        Username = "asmith",
                        Email = "asmith@example.com",
                        FirstName = "Alice",
                        LastName = "Smith",
                        CreatedBy = adminId,
                        CreatedAt = DateTime.Now,
                        LastModifiedBy = adminId,
                        LastModifiedAt = DateTime.Now
                    });
            }

            if (!_context.Projects.Any())
            {
                _context.Projects.AddRange(
                    new Project
                    {
                        Id = Guid.NewGuid(),
                        Name = "Project Alpha",
                        Description = "A test project for Alpha team",
                        StartDate = DateTime.UtcNow.AddDays(-30),
                        EndDate = DateTime.UtcNow.AddDays(60),
                        CreatedBy = adminId,
                        CreatedAt = DateTime.Now,
                        LastModifiedBy = adminId,
                        LastModifiedAt = DateTime.Now
                    },
                    new Project
                    {
                        Id = Guid.NewGuid(),
                        Name = "Project Beta",
                        Description = "A test project for Beta team",
                        StartDate = DateTime.UtcNow,
                        EndDate = DateTime.UtcNow.AddDays(90),
                        CreatedBy = adminId,
                        CreatedAt = DateTime.Now,
                        LastModifiedBy = adminId,
                        LastModifiedAt = DateTime.Now
                    });
            }

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
