using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.OpenApi.Models;
using Movie.User.Service.Api.Validators;
using System.Reflection;

namespace Movie.User.Service.Api;

/// <summary>
/// Classe de extensão para configuração de serviços da API
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adiciona os serviços da camada de API ao container de injeção de dependência
    /// </summary>
    /// <param name="services">Coleção de serviços</param>
    /// <returns>Coleção de serviços configurada</returns>
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        // Controllers
        AddControllers(services);

        // API Validators
        AddValidators(services);

        // Swagger/OpenAPI
        AddSwagger(services);

        // Health Checks
        AddHealthChecks(services);

        // CORS
        AddCorsPolicy(services);

        return services;
    }

    private static void AddControllers(IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            // Global filters
            options.Filters.Add<ValidationFilter>();
        })
        .ConfigureApiBehaviorOptions(options =>
        {
            // Custom model validation response
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .SelectMany(x => x.Value!.Errors)
                    .Select(x => x.ErrorMessage)
                    .ToList();

                return new BadRequestObjectResult(new
                {
                    Message = "Dados inválidos",
                    Errors = errors
                });
            };
        });
    }

    private static void AddValidators(IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddScoped<ValidationFilter>();
    }

    private static void AddSwagger(IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Movie User Service API",
                Version = "v1.0.0",
                Description = "API para gerenciamento de usuários do sistema de filmes com arquitetura limpa e DDD",
                Contact = new OpenApiContact
                {
                    Name = "Movie User Service Team",
                    Email = "dev@movieuserservice.com",
                    Url = new Uri("https://github.com/movieuserservice")
                },
                License = new OpenApiLicense
                {
                    Name = "MIT License",
                    Url = new Uri("https://opensource.org/licenses/MIT")
                }
            });

            // Include XML comments
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                c.IncludeXmlComments(xmlPath);
            }

            // Add security definition
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            // Add security requirement
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
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

            // Custom schema IDs
            c.CustomSchemaIds(type => type.FullName);
            
            // Add examples
            c.EnableAnnotations();
        });
    }

    private static void AddHealthChecks(IServiceCollection services)
    {
        services.AddHealthChecks()
            .AddDbContextCheck<Movie.User.Service.Infra.Data.ApplicationDbContext>("database");
    }

    private static void AddCorsPolicy(IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder =>
            {
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });

            options.AddPolicy("Production", builder =>
            {
                builder
                    .WithOrigins("https://movieapp.com", "https://admin.movieapp.com")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        });
    }

    /// <summary>
    /// Configura o pipeline de requisições HTTP da aplicação
    /// </summary>
    /// <param name="app">Aplicação web</param>
    /// <returns>Aplicação web configurada</returns>
    public static WebApplication ConfigureApiPipeline(this WebApplication app)
    {
        // Configure the HTTP request pipeline
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Movie User Service API v1.0.0");
                c.RoutePrefix = "swagger";
                c.DocumentTitle = "Movie User Service API - Swagger UI";
                c.DefaultModelsExpandDepth(-1); // Hide schemas section by default
                c.DefaultModelExpandDepth(2);
                c.DisplayRequestDuration();
                c.EnableDeepLinking();
                c.EnableFilter();
                c.ShowExtensions();
                c.EnableValidator();
                c.SupportedSubmitMethods(new[] { 
                    Swashbuckle.AspNetCore.SwaggerUI.SubmitMethod.Get, 
                    Swashbuckle.AspNetCore.SwaggerUI.SubmitMethod.Post, 
                    Swashbuckle.AspNetCore.SwaggerUI.SubmitMethod.Put, 
                    Swashbuckle.AspNetCore.SwaggerUI.SubmitMethod.Delete 
                });
            });
            app.UseCors("AllowAll");
        }
        else
        {
            app.UseHsts();
            app.UseCors("Production");
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();

        // Health checks
        app.MapHealthChecks("/health");

        // Controllers
        app.MapControllers();

        return app;
    }
}

/// <summary>
/// Filtro de validação para requisições da API
/// </summary>
public class ValidationFilter : ActionFilterAttribute
{
    /// <summary>
    /// Executa validação antes da ação do controller
    /// </summary>
    /// <param name="context">Contexto de execução da ação</param>
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .SelectMany(x => x.Value!.Errors)
                .Select(x => x.ErrorMessage)
                .ToList();

            context.Result = new BadRequestObjectResult(new
            {
                Message = "Dados inválidos",
                Errors = errors
            });
        }
    }
}