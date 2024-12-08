using ManagementSystem.Domain.Entities;
using ManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ManagementSystem.Infrastructure.Identity;
public class TokenProvider
{
    private readonly IConfiguration _configuration;
    private readonly ApplicationDbContext _dbContext;
    public TokenProvider (IConfiguration configuration, ApplicationDbContext dbContext)
    {
        _configuration = configuration;
        _dbContext = dbContext;
    }

    public string GenerateJwtToken (User user, IList<string> roles)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName)
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Issuer"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<string> GenerateRefreshToken (Guid userId)
    {
        var refreshTokenValue = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));

        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Token = refreshTokenValue,
            ExpiresOn = DateTime.UtcNow.AddDays(1)
        };

        _dbContext.RefreshTokens.Add(refreshToken);

        await _dbContext.SaveChangesAsync();

        return refreshToken.Token;
    }

    public async Task<(string newJwtToken, string newRefreshToken)> RefreshTokens (string refreshToken)
    {
        var storedToken = _dbContext.RefreshTokens.FirstOrDefault(rt => rt.Token == refreshToken);

        if (storedToken == null || storedToken.ExpiresOn < DateTime.UtcNow)
        {
            throw new UnauthorizedAccessException("Invalid refresh token or an old one");
        }

        var user = await _dbContext.Users.FindAsync(storedToken.UserId);

        if (user == null)
        {
            throw new UnauthorizedAccessException("User not found");
        }

        var roles = await _dbContext.UserRoles
            .Where(ur => ur.UserId == user.Id)
            .Join(_dbContext.Roles,
                    ur => ur.RoleId,
                    role => role.Id,
                    (ur, role) => role.Name)
            .ToListAsync();

        var newJwtToken = GenerateJwtToken(user, roles);
        var newRefreshToken = await GenerateRefreshToken(user.Id);

        storedToken.ExpiresOn = DateTime.UtcNow;

        _dbContext.RefreshTokens.Update(storedToken);

        await _dbContext.SaveChangesAsync();

        return (newJwtToken, newRefreshToken);
    }
}
