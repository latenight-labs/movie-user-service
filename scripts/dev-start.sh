#!/bin/bash

# ==============================================================================
# Development Environment Startup Script
# ==============================================================================

set -e

echo "🚀 Starting Movie User Service Development Environment..."

# Check if Docker is running
if ! docker info > /dev/null 2>&1; then
    echo "❌ Docker is not running. Please start Docker first."
    exit 1
fi

# Create logs directory if it doesn't exist
mkdir -p logs

# Copy environment file if it doesn't exist
if [ ! -f .env ]; then
    echo "📋 Creating .env file from template..."
    cp .env.example .env
    echo "⚠️  Please update .env file with your configuration before running in production!"
fi

# Build and start services
echo "🔨 Building and starting services..."
docker-compose up --build -d

# Wait for services to be healthy
echo "⏳ Waiting for services to be ready..."
sleep 10

# Check service health
echo "🔍 Checking service health..."
docker-compose ps

# Show useful information
echo ""
echo "✅ Development environment is ready!"
echo ""
echo "📊 Services:"
echo "  • API: http://localhost:8000"
echo "  • Swagger: http://localhost:8000/swagger"
echo "  • PostgreSQL: localhost:5432"
echo "  • pgAdmin: http://localhost:8080 (admin@movieuser.com / admin123)"
echo ""
echo "🔧 Useful commands:"
echo "  • View logs: docker-compose logs -f"
echo "  • Stop services: docker-compose down"
echo "  • Restart API: docker-compose restart movie-user-api"
echo "  • Database shell: docker-compose exec postgres psql -U movieuser -d movieuserservice"
echo ""