using ManagementSystem.Domain.Entities;
using ManagementSystem.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ManagementSystem.Infrastructure.Data;

public class DbContextInitialiser
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DbContextInitialiser> _logger;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    public DbContextInitialiser (ApplicationDbContext context,
        ILogger<DbContextInitialiser> logger,
        UserManager<User> userManager,
        RoleManager<IdentityRole<Guid>> roleManager)
    {
        _context = context;
        _logger = logger;

        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitialiseAsync ()
    {
        try
        {
            if ((await _context.Database.GetPendingMigrationsAsync()).Any())
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("An error occured while applying migrations.");
            throw;
        }
    }

    public async Task SeedAsync ()
    {
        try
        {
            foreach (var roleName in Enum.GetNames(typeof(ApplicationRole)))
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole<Guid> { Name = roleName });
                }
            }

            var adminEmail = "admin@test.com";
            var adminUser = await _userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new User
                {
                    UserName = "admin",
                    Email = adminEmail,
                    FirstName = "Administrator",
                    LastName = "User"
                };

                var result = await _userManager.CreateAsync(adminUser, "Admin123!");

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(adminUser, ApplicationRole.Admin.ToString());
                }
                else
                {
                    _logger.LogError("Admin creation failed");
                }
            }

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError("An error occured while seeding the database.");
            throw;
        }
    }
}
