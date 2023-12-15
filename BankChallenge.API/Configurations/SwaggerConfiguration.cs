using Microsoft.OpenApi.Models;
using System.Reflection;

namespace BankChallenge.API.Configurations;

public static class SwaggerConfiguration
{
    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(option =>
        {
            option.EnableAnnotations();
            option.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "BankChallenge.API",
                Description = "API em .NET 8 que simula algumas funcionalidades de um banco digital.",
                Version = "v1",
                Contact = new OpenApiContact
                {
                    Name = "Matheus de Assis Alcântara",
                    Url = new Uri("https://www.matheusassis.dev"),
                    Email = "contato@matheusassis.dev"
                }
            });
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Insira um JWT TOKEN válido.",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            option.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                    Array.Empty<string>()
                }
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            option.IncludeXmlComments(xmlPath);
        });
    }
}