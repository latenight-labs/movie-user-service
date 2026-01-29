using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Movie.User.Service.Domain.Repositories;
using Movie.User.Service.Infra.Configuration;
using Movie.User.Service.Infra.Data;
using Movie.User.Service.Infra.Repositories;

namespace Movie.User.Service.Infra;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Database Configuration
        AddDatabase(services, configuration);

        // Repositories
        AddRepositories(services);

        // Configuration Settings
        AddConfigurations(services, configuration);

        return services;
    }

    private static void AddDatabase(IServiceCollection services, IConfiguration configuration)
    {
        var databaseSettings = configuration.GetSection(DatabaseSettings.SectionName).Get<DatabaseSettings>()
            ?? new DatabaseSettings();

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                npgsqlOptions =>
                {
                    npgsqlOptions.CommandTimeout(databaseSettings.CommandTimeout);
                    npgsqlOptions.EnableRetryOnFailure(
                        maxRetryCount: databaseSettings.MaxRetryCount,
                        maxRetryDelay: databaseSettings.MaxRetryDelay,
                        errorCodesToAdd: null);
                });

            // Development configurations
            if (databaseSettings.EnableSensitiveDataLogging)
            {
                options.EnableSensitiveDataLogging();
            }

            if (databaseSettings.EnableDetailedErrors)
            {
                options.EnableDetailedErrors();
            }

            // SQL logging in development
            options.LogTo(Console.WriteLine, LogLevel.Information);
        });
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
    }

    private static void AddConfigurations(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DatabaseSettings>(configuration.GetSection(DatabaseSettings.SectionName));
    }

    public static async Task<IServiceProvider> MigrateDatabaseAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        try
        {
            await context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();
            logger.LogError(ex, "Erro ao aplicar migrations do banco de dados");
            throw;
        }

        return serviceProvider;
    }
}
