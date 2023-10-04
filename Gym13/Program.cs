using Gym13.Application.Interfaces;
using Gym13.Extensions;
using Gym13.Models;
using Gym13.Domain.Data;
using Gym13.Domain.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using Gym13;
using Gym13.Application.Models;
using Gym13.Application.Services;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Gym13.Application.Validators;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);
var migrationsAssembly = "Gym13.Domain";

IServiceCollection services = builder.Services;
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
services.AddDbContext<Gym13DbContext>(options => options.UseNpgsql(connectionString, x => x.MigrationsAssembly(migrationsAssembly)));

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

services.AddFluentValidationAutoValidation();
services.AddFluentValidationClientsideAdapters();
services.AddValidatorsFromAssemblies(new[] { Assembly.Load("Gym13.Application"), typeof(Program).Assembly });
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

//services.AddAuthentication(opt =>
//{
//    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//});

services.Configure<SmsSenderOptions>(builder.Configuration.GetSection(nameof(SmsSenderOptions)));

//services.AddSwaggerDocumentation(builder.Configuration);
services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        In = ParameterLocation.Header
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                            },
                            Array.Empty<string>()
                        }
                    });

    options.CustomSchemaIds(type => type.FullName);
});
services.AddCustomLocalization();
services.AddHttpContextAccessor();
services.AddMemoryCache();

X509Certificate2 cert = null;
var root = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
var keysPath = Path.Combine(root, "Keys");
var path = Path.Combine(keysPath, "gym13cert.pfx");

cert = new X509Certificate2(path, "gym13", X509KeyStorageFlags.MachineKeySet);

services.AddIdentityServer(options =>
{
    options.EmitStaticAudienceClaim = true;
}).AddSigningCredential(cert).
AddConfigurationStore(options => options.ConfigureDbContext = builder =>
builder.UseNpgsql(connectionString,
sql => sql.MigrationsAssembly(typeof(Gym13DbContext).GetTypeInfo().Assembly.GetName().Name)))
.AddOperationalStore(options =>
{
    options.ConfigureDbContext = builder => builder.UseNpgsql(connectionString, sql => sql.MigrationsAssembly(typeof(Gym13DbContext).GetTypeInfo().Assembly.GetName().Name));
    options.EnableTokenCleanup = true;
    options.TokenCleanupInterval = 1200;
    options.TokenCleanupBatchSize = 500;
})
.AddAspNetIdentity<ApplicationUser>()
.AddInMemoryApiResources(ClientConfiguration.GetApiResources())
.AddInMemoryClients(ClientConfiguration.GetClients())
.AddInMemoryApiScopes(ClientConfiguration.GetApiScopes());

//services.AddIdentityServer(opts =>
//{
//    opts.Events.RaiseSuccessEvents = true;
//    opts.IssuerUri = builder.Configuration.GetValue<string>($"IdentityServerConfig:Authority");
//})
//    .AddSigningCredential(cert)
//    .AddAspNetIdentity<ApplicationUser>()
//    .AddProfileService<ProfileService>()
//    .AddConfigurationStore(options =>
//    {
//        options.ConfigureDbContext = builder =>
//            builder.UseNpgsql(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
//    }).AddOperationalStore(options =>
//    {
//        options.ConfigureDbContext = builder =>
//            builder.UseNpgsql(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
//    })
//    .AddInMemoryPersistedGrants()
//    .AddInMemoryApiResources(ClientConfiguration.GetApiResources())
//    .AddInMemoryClients(ClientConfiguration.GetClients())
//    .AddInMemoryApiScopes(ClientConfiguration.GetApiScopes())
//    .AddResourceOwnerValidator<AppResourceOwnerPasswordValidator>()
//    .AddExtensionGrantValidator<SmsConfirmGrantValidator>()
//    .AddExtensionGrantValidator<FacebookGrantValidator>();

services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddIdentityServerAuthentication(options =>
{
    options.ApiName = "Gym13Client";
    options.ApiSecret = "Gym13Secret";
    options.Authority = "https://localhost:7258";
    options.RequireHttpsMetadata = false;
    options.JwtValidationClockSkew = TimeSpan.FromMinutes(1);
});
services.AddTransient<IProfileService, ProfileService>();
services.AddScoped<IPlanService, Gym13.Application.Services.PlanService>();
services.AddScoped<ITrainerService, TrainerService>();
services.AddScoped<IAccountService, AccountService>();
services.AddTransient<ISmsSender, MessageSender>();
services.AddTransient<IEmailSender, MessageSender>();
services.AddTransient<IBannerService, BannerService>();
services.AddTransient<IHttpClientWrapper, HttpClientWrapper>();

services.AddCors();

var app = builder.Build();

app.UseCors(builder =>
{
    builder.WithOrigins("http://localhost:3000")
    .AllowAnyMethod().AllowAnyHeader().AllowCredentials();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseIdentityServer();

app.UseSwaggerDocumentation(builder.Configuration);
app.UseRequestLocalization();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});

app.Run();
