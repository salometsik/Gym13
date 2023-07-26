using Gym13.Application.Interfaces;
using Gym13.Extensions;
using Gym13.Infrastructure.Services;
using Gym13.Models;
using Gym13.Persistence.Data;
using Gym13.Persistence.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
IServiceCollection services = builder.Services;
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
services.AddDbContext<Gym13DbContext>(options => options.UseNpgsql(connectionString));

services.AddControllers();

services.AddIdentity<ApplicationUser, IdentityRole>(o =>
{
    o.Password = new PasswordOptions
    {
        RequireDigit = false,
        RequireLowercase = false,
        RequireNonAlphanumeric = false,
        RequireUppercase = false,
        RequiredLength = 6
    };
})
.AddEntityFrameworkStores<Gym13DbContext>()
.AddDefaultTokenProviders()
.AddUserValidator<UserValidator>();

services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    // my API name as defined in Config.cs - new ApiResource... or in DB ApiResources table
    o.Audience = "user_registration";
    // URL of Auth server(API and Auth are separate projects/applications),
    o.Authority = builder.Configuration.GetValue<string>("IdentityServerConfig:Authority");
    o.RequireHttpsMetadata = true;
    o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateAudience = true,
        // Scopes supported by API as defined in Config.cs - new ApiResource... or in DB ApiScopes table
        ValidAudiences = new List<string>() {
                        "user_registration",
                        "Gym13Api"
            },
        ValidateIssuer = true
    };
});

services.AddSwaggerDocumentation(builder.Configuration);
services.AddHttpContextAccessor();
services.AddMemoryCache();

services.AddScoped<IGymService, GymService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
