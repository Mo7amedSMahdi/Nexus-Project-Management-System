using Nexus.Core.DTOs.Identity;

namespace Nexus.Core.Interfaces.Identity;

public interface IIdentityService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<AuthResponse?> GetByIdAsync(string id);
    Task<List<AuthResponse>> GetAllAsync();
}