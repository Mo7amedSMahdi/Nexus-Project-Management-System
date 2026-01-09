namespace Nexus.Core.Interfaces.Security;

public interface ICurrentUser
{
    string UserId { get; }
    string Email { get; }
    string FirstName { get; }
    string LastName { get; }
    string? AvatarUrl { get; }
    string? Bio { get; }
}