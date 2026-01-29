#!/bin/bash

# ==============================================================================
# Development Environment Shutdown Script
# ==============================================================================

set -e

echo "🛑 Stopping Movie User Service Development Environment..."

# Stop and remove containers
docker-compose down

# Optional: Remove volumes (uncomment if you want to reset database)
# echo "🗑️  Removing volumes..."
# docker-compose down -v

echo "✅ Development environment stopped!"
echo ""
echo "💡 To completely reset the environment (including database), run:"
echo "   docker-compose down -v --remove-orphans"
echo ""