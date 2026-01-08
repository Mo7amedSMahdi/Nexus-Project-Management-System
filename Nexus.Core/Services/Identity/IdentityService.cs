using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Nexus.Core.DTOs.Identity;
using Nexus.Core.Entities.Identity;
using Nexus.Core.Interfaces.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Nexus.Core.Services.Identity;

public class IdentityService(UserManager<ApplicationUser> _userManager, IConfiguration _configuration) : IIdentityService
{
    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            AvatarUrl = request.AvatarUrl,
            Bio = request.Bio
        };
        var result = await _userManager.CreateAsync(user,request.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ",result.Errors.Select(e => e.Description));
            throw new Exception(errors);
        }

        return GenerateAuthResponse(user);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null) throw new Exception("Invalid credentials");
        
        var result = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!result) throw new Exception("Invalid credentials");
        
        return GenerateAuthResponse(user);
    }

    public Task<AuthResponse?> GetByIdAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<List<AuthResponse>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    private AuthResponse GenerateAuthResponse(ApplicationUser user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Key"]!);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }),
            Expires = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["JwtSettings:DurationInMinutes"]!)),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _configuration["JwtSettings:Issuer"],
            Audience = _configuration["JwtSettings:Audience"]
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return new AuthResponse
        {
            Id = user.Id,
            Email = user.Email!,
            Token = tokenHandler.WriteToken(token),
            FirstName = user.FirstName,
            LastName = user.LastName,
            AvatarUrl = user.AvatarUrl ?? string.Empty,
            Bio = user.Bio ?? string.Empty
        };
    }
}