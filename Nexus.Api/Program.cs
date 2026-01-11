using System.Reflection;
using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Nexus.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Nexus.Core.Entities.Identity;
using Nexus.Core.Interfaces.Identity;
using Scalar.AspNetCore;
//projects
using Nexus.Core.Interfaces.Projects;
using Nexus.Core.Interfaces.Security;
using Nexus.Infrastructure.Repositories.Projects;
using Nexus.Core.Services.Projects;
// tickets
using Nexus.Core.Interfaces.Tickets;
using Nexus.Core.Services.Identity;
using Nexus.Core.Services.Security;
using Nexus.Core.Services.Tickets;
using Nexus.Infrastructure.Repositories.Tickets;

var builder = WebApplication.CreateBuilder(args);

// Database Configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<NexusDbContext>(options => options.UseNpgsql(connectionString));

// Register Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<NexusDbContext>()
    .AddDefaultTokenProviders();

// Configure JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// --- Dependency Injection Registration ---

builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<ITicketRepository, TicketRepository>();

// --- Register Service ---
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<ICurrentUser, CurrentUser>();

builder.Services.AddHttpContextAccessor();
// -----------------------------------------

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "Nexus.Api", Version = "v1" });
    
    // add Jwt requirements in swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\""
    });
    
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new List<string>() // <--- CHANGED: Must be a List, not an Array
        }
    });
    
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(
        Path.Combine(AppContext.BaseDirectory, xmlFilename)
    );
});

// Register fluent validators
builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();



var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

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
        options.ConfigObject.AdditionalItems["persistAuthorization"] = true;
    });
    
    // Scalar UI 
    // 3. Serve Scalar UI (The modern look)
    // URL: https://localhost:xxxx/scalar/v1
    app.MapScalarApiReference(options =>
    {
        // Tell Scalar to use the JSON generated by Swashbuckle
        options.WithOpenApiRoutePattern("/swagger/v1/swagger.json");
        
        options.WithTitle("Nexus API Docs");
        options.WithTheme(ScalarTheme.DeepSpace); // Themes: Mars, Moon, DeepSpace, etc.
        options.WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient); // Set default code snippet
    });
}

app.UseHttpsRedirection();

app.Run();

