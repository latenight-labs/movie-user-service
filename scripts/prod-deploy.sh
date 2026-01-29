#!/bin/bash

# ==============================================================================
# Production Deployment Script
# ==============================================================================

set -e

echo "🚀 Deploying Movie User Service to Production..."

# Check if .env file exists
if [ ! -f .env ]; then
    echo "❌ .env file not found. Please create it from .env.example"
    exit 1
fi

# Check required environment variables
if [ -z "$POSTGRES_PASSWORD" ]; then
    echo "❌ POSTGRES_PASSWORD environment variable is required"
    exit 1
fi

# Create necessary directories
mkdir -p logs
mkdir -p backups

# Build and deploy
echo "🔨 Building and deploying services..."
docker-compose -f docker-compose.prod.yml up --build -d

# Wait for services to be healthy
echo "⏳ Waiting for services to be ready..."
sleep 30

# Check service health
echo "🔍 Checking service health..."
docker-compose -f docker-compose.prod.yml ps

# Run database migrations
echo "🗄️  Running database migrations..."
docker-compose -f docker-compose.prod.yml exec movie-user-api-prod dotnet ef database update --no-build

echo ""
echo "✅ Production deployment completed!"
echo ""
echo "📊 Services:"
echo "  • API: http://localhost:8000"
echo "  • Nginx: http://localhost:80"
echo ""
echo "🔧 Useful commands:"
echo "  • View logs: docker-compose -f docker-compose.prod.yml logs -f"
echo "  • Stop services: docker-compose -f docker-compose.prod.yml down"
echo "  • Backup database: docker-compose -f docker-compose.prod.yml exec postgres pg_dump -U movieuser movieuserservice > backups/backup_\$(date +%Y%m%d_%H%M%S).sql"
echo ""