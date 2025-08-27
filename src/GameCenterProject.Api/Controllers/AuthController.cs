using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GameCenterProject.Entities;
using GameCenterProject.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace GameCenterProject.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _cfg;

    public AuthController(AppDbContext db, IConfiguration cfg)
    {
        _db = db; _cfg = cfg;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest dto)
    {
        if (await _db.Users.AnyAsync(u => u.Email == dto.Email))
            return BadRequest("Email already exists.");


        string role;
        if (string.IsNullOrWhiteSpace(dto.Role))
            role = "user";
        else
            role = dto.Role.ToLower();

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = dto.Email,
            DisplayName = dto.DisplayName,
            PasswordHash = HashPassword(dto.Password),
            CreatedAt = DateTime.UtcNow
        };
        user.Role = role;

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return Ok(new { message = "registered" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest dto)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (user == null || !VerifyPassword(dto.Password, user.PasswordHash))
            return Unauthorized("Invalid credentials");

        var token = GenerateJwt(user);
        return Ok(new
        {
            token,
            user = new { id = user.Id, email = user.Email, displayName = user.DisplayName, role = user.Role }
        });
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
        if (user is null) return Unauthorized();
    return Ok(new { id = user.Id, email = user.Email, displayName = user.DisplayName, role = user.Role });
    }

    private string GenerateJwt(User user)
    {
        var issuer = _cfg["Jwt:Issuer"]!;
        var audience = _cfg["Jwt:Audience"]!;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_cfg["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
            new Claim(ClaimTypes.Name, user.DisplayName ?? user.Email ?? ""),
            new Claim(ClaimTypes.Role, user.Role ?? "user"),
        };

        var token = new JwtSecurityToken(issuer, audience, claims,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(_cfg["Jwt:ExpiryMinutes"] ?? "60")),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string HashPassword(string password)
    {
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    private bool VerifyPassword(string password, string hash)
    {
        return HashPassword(password) == hash;
    }

    public record RegisterRequest(string Email, string Password, string DisplayName, string? Role = null);
    public record LoginRequest(string Email, string Password);
}
