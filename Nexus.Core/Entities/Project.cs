namespace Nexus.Core.Entities;

public class Project : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    
    // TODO: Add the Ticket relationship

    // empty constructure for EF core
    public Project()
    {
    }
    
    // Constructor for Us (Enforce rules)
    public Project(string name, string code, string? description = null)
    {
        Name = name;
        Code = code.ToUpper();
        Description = description;
        CreatedAt = DateTime.UtcNow;
    }
    
    

}