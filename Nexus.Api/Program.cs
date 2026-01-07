using System.Reflection;
using Nexus.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
//projects
using Nexus.Core.Interfaces.Projects;
using Nexus.Core.Interfaces.Tickets;
using Nexus.Infrastructure.Repositories.Projects;
using Nexus.Core.Services.Projects;
// tickets
using Nexus.Core.Interfaces.Tickets;
using Nexus.Core.Services.Tickets;
using Nexus.Infrastructure.Repositories.Tickets;

var builder = WebApplication.CreateBuilder(args);

// Database Configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<NexusDbContext>(options => options.UseNpgsql(connectionString));

// --- Dependency Injection Registration ---

builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<ITicketRepository, TicketRepository>();

// --- Register Service ---
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ITicketService, TicketService>();
// -----------------------------------------

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "Nexus.Api", Version = "v1" });
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(
        Path.Combine(AppContext.BaseDirectory, xmlFilename)
    );
});

var app = builder.Build();

app.MapControllers();

// --- Database Seeding Block ---
// We create a temporary scope to get the DbContext
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<NexusDbContext>();
        
        // Optional: Ensure database is created/migrated automatically
        // await context.Database.MigrateAsync(); 

        // Run the Seeder
        await NexusContextSeed.SeedAsync(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}
// -----------------------------


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Nexus.Api v1");
        options.RoutePrefix = "swagger"; // /swagger
    });
}

app.UseHttpsRedirection();

app.Run();

