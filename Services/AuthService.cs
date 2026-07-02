using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TodoApi.Data;
using TodoApi.Models;
namespace TodoApi.Services;

public class AuthService(AppDbContext context, IConfiguration configuration) : IAuthService
{
    public async Task<User?> RegisterAsync(string email, string password)
    {
       
        if (await context.Users.AnyAsync(x => x.Email == email)) return null;
        var hashedHash =  BCrypt.Net.BCrypt.HashPassword(password);
        var newUser = new User
        {
            Email = email,
            PasswordHash = hashedHash,
        };
        await context.Users.AddAsync(newUser);
        await context.SaveChangesAsync();
        return newUser;
    }

    public async Task<string?> LoginAsync(string email, string password)
    {
        //find user
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null) return null;
        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash)) return null;
        return GenerateJwtToken(user);
    }
    
    private string GenerateJwtToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured")));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(double.Parse(configuration["Jwt:ExpiryMinutes"] ?? "60")),
            signingCredentials: credentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}