using Nexus.Core.Entities.Tickets;

namespace Nexus.Core.Entities.Projects;

public class Project(string name, string code,string ownerId, string? description = null) : BaseEntity
{
    public string Name { get; set; } = name;
    public string Code { get; set; } = code.ToUpper();
    public string? Description { get; set; } = description;
    
    public string OwnerId { get; set; } = ownerId;

    // One project connects to Many Tickets
    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    // empty constructure for EF core
    public Project() : this(name:string.Empty,code:string.Empty,ownerId:string.Empty)
    {
    }

}