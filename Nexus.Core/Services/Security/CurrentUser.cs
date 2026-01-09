using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Nexus.Core.Interfaces.Security;

namespace Nexus.Core.Services.Security;

public class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    public string UserId => httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)!;
    public string Email => httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email)!;
    public string FirstName => httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.GivenName)!;
    public string LastName => httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Surname)!;
    public string? AvatarUrl => httpContextAccessor.HttpContext?.User?.FindFirstValue("avatar_url");
    public string? Bio => httpContextAccessor.HttpContext?.User?.FindFirstValue("bio");
}