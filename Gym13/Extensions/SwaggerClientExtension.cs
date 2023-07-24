using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Gym13.Extensions
{
    public static class SwaggerClientExtension
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services, IConfiguration configuration)
        {
            var apiHost = configuration.GetValue<string>("IdentityServerOptions:Authority").TrimEnd('/') + "/";

            services.AddSwaggerGen(c =>
            {
                c.MapType<decimal>(() => new OpenApiSchema { Type = "number", Format = "decimal" });
                c.UseInlineDefinitionsForEnums();

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Gym 13 Api",
                    Version = "v1",
                    Contact = new OpenApiContact
                    {
                        Email = "support@gym13.ge"
                    },
                    Description = ""
                });

                c.OperationFilter<AddOperationId>();
                c.OperationFilter<AddLanguageHeaderParameter>();
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        ClientCredentials = new OpenApiOAuthFlow
                        {
                            TokenUrl = new Uri($"{apiHost}connect/token", UriKind.Absolute)
                        },
                        Password = new OpenApiOAuthFlow
                        {
                            TokenUrl = new Uri($"{apiHost}connect/token", UriKind.Absolute)
                        }
                    }
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "oauth2"
                            }
                        },
                       new string[] {  }
                    }
                });
                c.CustomSchemaIds((type) => type.Name);
                //c.AddServer(new OpenApiServer { Url = apiHost });
                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //c.IncludeXmlComments(xmlPath);
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app, IConfiguration configuration/*, List<string> ips, IWebHostEnvironment env*/)
        {
            var clientId = configuration.GetValue<string>("IdentityServerOptions:AuthClientId");
            var clientSecret = configuration.GetValue<string>("IdentityServerOptions:AuthClientSecret");
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = string.Empty;
                c.DocumentTitle = "Gym 13 API";
                c.DocExpansion(DocExpansion.None);

                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");

                //security vulnerability!!
#if DEBUG
                c.OAuthClientId(clientId);
                c.OAuthClientSecret(clientSecret);
#endif
                c.RoutePrefix = string.Empty;
            });

            return app;
        }
    }
    public class AddOperationId : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.OperationId = context.MethodInfo.Name;
        }
    }

    public class AddLanguageHeaderParameter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "accept-language",
                In = ParameterLocation.Header,
                Style = ParameterStyle.Simple,
                Schema = new OpenApiSchema { Type = "string", Nullable = true },
                Required = false
            });
        }
    }
}