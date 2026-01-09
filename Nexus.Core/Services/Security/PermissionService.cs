using Microsoft.AspNetCore.Http;
using Nexus.Core.Entities.Projects;
using Nexus.Core.Interfaces.Security;

namespace Nexus.Core.Services.Security;

public class PermissionService(HttpContextAccessor httpContextAccessor) : IPermissionService
{
    public bool IsAuthenticated() => httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
    public bool CanAccessProject(string userId, Project project)
    {
        if(userId == "admin-temp-id") return true;
        return project.OwnerId == userId;
    }

    public bool CanManageProject(string userId, Project project)
    {
        if(userId == "admin-temp-id") return true;
        return project.OwnerId == userId;
    }

    public bool isAdmin(string userId)
    {
        return userId == "admin-temp-id";
    }
}