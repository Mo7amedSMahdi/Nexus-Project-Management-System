using Nexus.Core.Entities.Projects;

namespace Nexus.Core.Interfaces.Security;

public interface IPermissionService
{
    bool IsAuthenticated();
    bool CanAccessProject(string userId, Project project);
    bool CanManageProject(string userId, Project project);
    bool isAdmin(string userId);
}