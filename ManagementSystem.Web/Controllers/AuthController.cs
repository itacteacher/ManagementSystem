using ManagementSystem.Domain.Entities;
using ManagementSystem.Domain.Enums;
using ManagementSystem.Infrastructure;
using ManagementSystem.Infrastructure.Emails;
using ManagementSystem.Infrastructure.Identity;
using ManagementSystem.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ManagementSystem.Web.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly TokenProvider _tokenProvider;
    private readonly IEmailSender _emailSender;
    private readonly EmailTemplateService _emailTemplateService;

    public AuthController (UserManager<User> userManager, TokenProvider tokenProvider, IEmailSender emailSender,
    EmailTemplateService emailTemplateService)
    {
        _userManager = userManager;
        _tokenProvider = tokenProvider;
        _emailSender = emailSender;
        _emailTemplateService = emailTemplateService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register ([FromBody] RegisterModel model)
    {
        var user = new User
        {
            UserName = model.UserName,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        await _userManager.AddToRoleAsync(user, ApplicationRole.User.ToIdentityRole());

        var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var confirmEmailUrl = Url.Action("ConfirmEmail", "Auth", new { userId = user.Id, token = emailConfirmationToken }, Request.Scheme);

        var emailSent = await SendEmailAsync(
            user.Email,
            "ConfirmEmail",
            new Dictionary<string, string>
            {
                { "UserName", user.UserName },
                { "ConfirmationLink", confirmEmailUrl }
            });

        return Ok(new { Message = "User registered successfully. Please confirm your email." });
    }

    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmEmail (string userId, string token)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return BadRequest("User not found.");
        }

        var result = await _userManager.ConfirmEmailAsync(user, token);
        if (!result.Succeeded)
        {
            return BadRequest("Email confirmation failed.");
        }

        return Ok(new { Message = "Email confirmed successfully." });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login ([FromBody] LoginModel model)
    {
        var user = await _userManager.FindByNameAsync(model.UserName);

        if (user == null)
        {
            return Unauthorized();
        }

        var isValidPassword = await _userManager.CheckPasswordAsync(user, model.Password);

        if (!isValidPassword)
        {
            return Unauthorized();
        }

        var roles = await _userManager.GetRolesAsync(user);

        var token = _tokenProvider.GenerateJwtToken(user, roles);
        var refreshToken = await _tokenProvider.GenerateRefreshToken(user.Id);

        return Ok(new { Token = token, RefreshToken = refreshToken });
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken ([FromBody] RefreshTokenModel model)
    {
        try
        {
            var (newJwtToken, newRefreshToken) = await _tokenProvider.RefreshTokens(model.RefreshToken);

            return Ok(new { Token = newJwtToken, RefreshToken = newRefreshToken });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { Error = ex.Message });
        }
    }

    private async Task<bool> SendEmailAsync (string recipientEmail, string templateName, Dictionary<string, string> replacements)
    {
        var template = _emailTemplateService.GetTemplateByName(templateName);

        if (template == null)
        {
            return false;
        }

        var body = EmailTemplateHelper.PopulateTemplate(template.Body, replacements);

        await _emailSender.SendEmailAsync(recipientEmail, template.Subject, body);
        return true;
    }
}
