using Gym13.Application.Interfaces;
using Gym13.Extensions;
using Gym13.Infrastructure.Services;
using Gym13.Models;
using Gym13.Persistence.Data;
using Gym13.Persistence.DTOs;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);
var migrationsAssembly = "Gym13.Persistence";

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

//X509Certificate2 cert = null;
//var root = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
//var keysPath = Path.Combine(root, "Keys");
//var path = Path.Combine(keysPath, "accountsscert.pfx");

//cert = new X509Certificate2(path, "account123", X509KeyStorageFlags.MachineKeySet);

services.AddIdentityServer(opts =>
{
    opts.Events.RaiseSuccessEvents = true;
    opts.IssuerUri = builder.Configuration.GetValue<string>($"IdentityServerConfig:Authority");
})
    //.AddSigningCredential(cert)
    .AddInMemoryPersistedGrants()
    .AddAspNetIdentity<ApplicationUser>()
    .AddProfileService<Gym13.ProfileService>()
    .AddConfigurationStore(options =>
    {
        options.ConfigureDbContext = builder =>
            builder.UseNpgsql(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
    }).AddOperationalStore(options =>
    {
        options.ConfigureDbContext = builder =>
            builder.UseNpgsql(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
    })
    //.AddInMemoryApiResources(ClientConfiguration.GetApiResources())
    //.AddInMemoryClients(ClientConfiguration.GetClients())
    .AddResourceOwnerValidator<AppResourceOwnerPasswordValidator>()
    .AddExtensionGrantValidator<SmsConfirmGrantValidator>()
    .Services
    .AddScoped<IExtensionGrantValidator, FacebookGrantValidator>();

services.AddTransient<IProfileService, Gym13.ProfileService>();
services.AddScoped<IGymService, GymService>();

services.AddCors();

var app = builder.Build();

app.UseCors(builder =>
{
    builder.WithOrigins("http://localhost:3000")
    .AllowAnyMethod().AllowAnyHeader().AllowCredentials();
});

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwaggerDocumentation(builder.Configuration);
app.UseRequestLocalization();

app.UseHttpsRedirection();

app.UseRouting();
app.UseIdentityServer();
app.UseAuthentication();

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});

app.Run();
