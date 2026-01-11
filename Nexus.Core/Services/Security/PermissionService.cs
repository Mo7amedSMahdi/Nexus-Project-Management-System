using Microsoft.AspNetCore.Http;
using Nexus.Core.Entities.Projects;
using Nexus.Core.Interfaces.Security;

namespace Nexus.Core.Services.Security;

public class PermissionService(IHttpContextAccessor httpContextAccessor) : IPermissionService
{
    public bool IsAuthenticated() => httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
    public bool CanAccessProject(string userId, Project project)
    {
        if(userId == "fdc4579b-5543-46d2-a271-34dc7e455aa3") return true;
        return project.OwnerId == userId;
    }

    public bool CanManageProject(string userId, Project project)
    {
        if(userId == "fdc4579b-5543-46d2-a271-34dc7e455aa3") return true;
        return project.OwnerId == userId;
    }

    public bool isAdmin(string userId)
    {
        return userId == "fdc4579b-5543-46d2-a271-34dc7e455aa3";
    }
}