using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProductCatalog.Api.Options;
using ProductCatalog.Application.DTOs.Auth;

namespace ProductCatalog.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/auth")]
public class AuthController : ControllerBase
{
    private readonly JwtOptions _jwtOptions;

    public AuthController(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }

    [HttpPost("login")]
    public ActionResult<AuthResponse> Login(LoginRequest request)
    {
        // POC user only. In production, validate against Identity DB / Entra ID / IAM provider.
        var users = new Dictionary<string, (string Password, string Role)>
{
    { "admin", ("Admin@123", "Admin") },
    { "guest", ("Guest@123", "Guest") }
};

        if (!users.TryGetValue(request.UserName, out var user) ||
            user.Password != request.Password)
        {
            return Unauthorized();
        }

        var accessToken = GenerateAccessToken(request.UserName, user.Role);
        var refreshToken = GenerateRefreshToken();

        return Ok(new AuthResponse(
            accessToken,
            refreshToken,
            DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenExpiryMinutes)));
    }

    [HttpPost("refresh-token")]
    public ActionResult<AuthResponse> RefreshToken(RefreshTokenRequest request)
    {
        // POC only. In production, refresh tokens must be stored, hashed, rotated, and revoked.
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
        {
            return Unauthorized();
        }

        var accessToken = GenerateAccessToken("admin", "Admin");
        var refreshToken = GenerateRefreshToken();

        return Ok(new AuthResponse(
            accessToken,
            refreshToken,
            DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenExpiryMinutes)));
    }

    private string GenerateAccessToken(string userName, string role)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, userName),
            new(ClaimTypes.Role, role),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenExpiryMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static string GenerateRefreshToken()
    {
        var randomBytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(randomBytes);
    }
}