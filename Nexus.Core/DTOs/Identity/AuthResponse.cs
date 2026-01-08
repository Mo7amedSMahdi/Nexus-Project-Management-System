namespace Nexus.Core.DTOs.Identity;

public class AuthResponse
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string AvatarUrl { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
}