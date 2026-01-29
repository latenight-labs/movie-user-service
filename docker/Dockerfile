# ==============================================================================
# Multi-stage Dockerfile for Movie User Service API
# ==============================================================================

# Base image for runtime - Using standard .NET runtime with ICU support
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8000

# Install curl for health checks
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Create non-root user for security
RUN groupadd -r appgroup && useradd -r -g appgroup appuser

# ==============================================================================
# Build stage
# ==============================================================================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

# Copy solution and project files first (for better layer caching)
COPY ["src/Movie.User.Service.Api/Movie.User.Service.Api.csproj", "src/Movie.User.Service.Api/"]
COPY ["src/Movie.User.Service.Service/Movie.User.Service.Service.csproj", "src/Movie.User.Service.Service/"]
COPY ["src/Movie.User.Service.Infra/Movie.User.Service.Infra.csproj", "src/Movie.User.Service.Infra/"]
COPY ["src/Movie.Usar.Service.Domain/Movie.User.Service.Domain.csproj", "src/Movie.Usar.Service.Domain/"]

# Restore dependencies
RUN dotnet restore "src/Movie.User.Service.Api/Movie.User.Service.Api.csproj"

# Copy source code
COPY src/ src/

# Build the application
WORKDIR "/src/src/Movie.User.Service.Api"
RUN dotnet build "Movie.User.Service.Api.csproj" -c Release -o /app/build --no-restore

# ==============================================================================
# Publish stage
# ==============================================================================
FROM build AS publish
RUN dotnet publish "Movie.User.Service.Api.csproj" -c Release -o /app/publish --no-restore \
    --self-contained false \
    --verbosity minimal

# Remove unnecessary files to reduce image size
RUN find /app/publish -name "*.pdb" -delete && \
    find /app/publish -name "*.xml" -delete

# ==============================================================================
# Final stage
# ==============================================================================
FROM base AS final

# Copy published application
COPY --from=publish /app/publish .

# Create logs directory
RUN mkdir -p /app/logs && \
    chown -R appuser:appgroup /app/logs

# Switch to non-root user
USER appuser

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8000
ENV DOTNET_RUNNING_IN_CONTAINER=true
ENV ASPNETCORE_ENVIRONMENT=Docker

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=60s --retries=3 \
    CMD curl -f http://localhost:8000/health || exit 1

ENTRYPOINT ["dotnet", "Movie.User.Service.Api.dll"]