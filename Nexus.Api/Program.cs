using System.Reflection;
using Nexus.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Nexus.Core.Interfaces.Projects;
using Nexus.Infrastructure.Repositories.Projects;
using Nexus.Core.Services.Projects;

var builder = WebApplication.CreateBuilder(args);

// Database Configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<NexusDbContext>(options => options.UseNpgsql(connectionString));

// --- Dependency Injection Registration ---

builder.Services.AddScoped<IProjectRepository, ProjectRepository>();

// --- Register Service ---
builder.Services.AddScoped<IProjectService, ProjectService>();
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

