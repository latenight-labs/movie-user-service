using Movie.User.Service.Api;
using Movie.User.Service.Infra;
using Movie.User.Service.Service;

var builder = WebApplication.CreateBuilder(args);

// ===================================================================
// DEPENDENCY INJECTION - Organized by Layer
// ===================================================================

// 🌐 API Layer - Controllers, Swagger, Validation, CORS
builder.Services.AddApiServices();

// 🏗️ Application Layer - MediatR, Handlers, Validators, Behaviors
builder.Services.AddApplicationServices();

// 🗄️ Infrastructure Layer - Database, Repositories, External Services
builder.Services.AddInfrastructureServices(builder.Configuration);

// ===================================================================
// BUILD APPLICATION
// ===================================================================
var app = builder.Build();

// ===================================================================
// CONFIGURE PIPELINE - Organized by Layer
// ===================================================================

// 🌐 API Pipeline Configuration
app.ConfigureApiPipeline();

// ===================================================================
// DATABASE MIGRATION (Development/Production)
// ===================================================================
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    try
    {
        await app.Services.MigrateDatabaseAsync();
        app.Logger.LogInformation("✅ Database migrations applied successfully");
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "❌ Error applying database migrations");
        throw;
    }
}

// ===================================================================
// START APPLICATION
// ===================================================================
app.Logger.LogInformation("🚀 Movie User Service API starting...");
app.Logger.LogInformation("📊 Environment: {Environment}", app.Environment.EnvironmentName);
app.Logger.LogInformation("🌐 URLs: {Urls}", string.Join(", ", builder.WebHost.GetSetting("urls")?.Split(';') ?? new[] { "Not configured" }));

// Open Swagger in Chrome automatically in Development
if (app.Environment.IsDevelopment())
{
    var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
    lifetime.ApplicationStarted.Register(() =>
    {
        try
        {
            var urls = builder.WebHost.GetSetting("urls")?.Split(';') ?? new[] { "http://localhost:5000" };
            var baseUrl = urls.FirstOrDefault()?.Replace("*", "localhost") ?? "http://localhost:5000";
            var swaggerUrl = $"{baseUrl}/swagger/index.html";
            
            app.Logger.LogInformation("🌐 Opening Swagger UI at: {SwaggerUrl}", swaggerUrl);
            
            // Try to open Chrome with Swagger URL
            var processStartInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "cmd",
                Arguments = $"/c start chrome \"{swaggerUrl}\"",
                UseShellExecute = false,
                CreateNoWindow = true
            };
            
            System.Diagnostics.Process.Start(processStartInfo);
        }
        catch (Exception ex)
        {
            app.Logger.LogWarning(ex, "⚠️ Could not automatically open Swagger in Chrome");
        }
    });
}

app.Run();
