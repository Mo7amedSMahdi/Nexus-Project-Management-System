using Nexus.Core.Entities.Projects;
using Nexus.Core.Entities.Tickets;
using Nexus.Core.Enums;
using Nexus.Core.Enums.Tickets;

namespace Nexus.Infrastructure.Data
{
    public static class NexusContextSeed
    {
        public static async Task SeedAsync(NexusDbContext context)
        {
            // 1. Check if the database is already populated
            // If we have projects, we assume we don't need to seed
            if (context.Projects.Any()) 
            {
                return; 
            }

            // 2. Create Sample Projects
            var projects = new List<Project>
            {
                new Project("Phoenix Project", "PHX",Guid.NewGuid().ToString(), "Rebuilding the legacy banking system from scratch."),
                new Project("Project Manhattan", "MAN",Guid.NewGuid().ToString(), "Top secret AI research initiative."),
                new Project("Apollo 11", "APL",Guid.NewGuid().ToString(), "The moon landing mission control software.")
            };

            await context.Projects.AddRangeAsync(projects);
            await context.SaveChangesAsync(); // Save to generate IDs

            // 3. Create Sample Tickets linked to those Projects
            // We use the objects directly or their IDs (projects[0].Id)
            var tickets = new List<Ticket>
            {
                // Phoenix Project Tickets
                new Ticket(title: "Design Database Schema", projectId: projects[0].Id, priority: TicketPriority.High) 
                    { Description = "Must support sharding.", Status = TicketStatus.Done },
                new Ticket("Setup CI/CD Pipeline", projects[0].Id, TicketPriority.Critical) 
                    { Description = "Use GitHub Actions.", Status = TicketStatus.InReview },
                new Ticket("Implement Login API", projects[0].Id, TicketPriority.Medium) 
                    { Status = TicketStatus.Todo },

                // Project Manhattan Tickets
                new Ticket("Train Initial Model", projects[1].Id, TicketPriority.Critical) 
                    { Description = "Need 4x H100 GPUs.", Status = TicketStatus.InProgress },
                new Ticket("Clean Dataset", projects[1].Id, TicketPriority.Low) 
                    { Status = TicketStatus.Todo },
                
                // Apollo 11 Tickets
                new Ticket("Calculate Trajectory", projects[2].Id, TicketPriority.Critical) 
                    { Description = "Physics engine integration.", Status = TicketStatus.Done }
            };

            await context.Tickets.AddRangeAsync(tickets);
            await context.SaveChangesAsync();
        }
    }
}