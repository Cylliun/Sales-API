using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SalesApi.Data;
using SalesApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SalesApi.Authentication;

public class AuthService
{
    private readonly JwtSettings _jwtSettings;
    private readonly DataContext _context;

    public AuthService(IOptions<JwtSettings> options, DataContext context)
    {
        _jwtSettings = options.Value
            ?? throw new ArgumentNullException(nameof(options));

        if (string.IsNullOrWhiteSpace(_jwtSettings.SecretKey))
            throw new ArgumentException("JWT SecretKey não pode ser vazia.");

        if (_jwtSettings.SecretKey.Length < 32)
            throw new ArgumentException("JWT SecretKey deve ter pelo menos 32 caracteres.");

        _context = context;

    }

    public string GenerateToken(string userId, string email, string role)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    public async Task<string?> LoginAsync(string email, string password)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);

        if (user == null)
            return null;

        var passwordValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);

        if (!passwordValid)
            return null;

        if (!user.IsActive)
            return null;

        var token = GenerateToken(
            user.Id.ToString(),
            user.Email,
            user.Role.ToString()
        );

        return token;
    }


}