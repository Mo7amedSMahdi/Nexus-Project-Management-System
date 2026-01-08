using Microsoft.AspNetCore.Identity;

namespace Nexus.Core.Entities.Identity;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; } = null;
    public string? Bio { get; set; } = null;
}