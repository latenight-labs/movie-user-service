# ==============================================================================
# Makefile for Movie User Service
# ==============================================================================

.PHONY: help dev-start dev-stop dev-logs dev-restart prod-deploy prod-stop build clean test

# Default target
help: ## Show this help message
	@echo "Movie User Service - Available Commands:"
	@echo ""
	@grep -E '^[a-zA-Z_-]+:.*?## .*$$' $(MAKEFILE_LIST) | sort | awk 'BEGIN {FS = ":.*?## "}; {printf "  \033[36m%-20s\033[0m %s\n", $$1, $$2}'
	@echo ""

# Development commands
dev-start: ## Start development environment
	@chmod +x scripts/dev-start.sh
	@./scripts/dev-start.sh

dev-stop: ## Stop development environment
	@chmod +x scripts/dev-stop.sh
	@./scripts/dev-stop.sh

dev-logs: ## Show development logs
	@docker-compose logs -f

dev-restart: ## Restart development API service
	@docker-compose restart movie-user-api

dev-shell: ## Access API container shell
	@docker-compose exec movie-user-api sh

dev-db: ## Access PostgreSQL shell
	@docker-compose exec postgres psql -U movieuser -d movieuserservice

dev-reset: ## Reset development environment (removes volumes)
	@docker-compose down -v --remove-orphans
	@docker system prune -f

# Production commands
prod-deploy: ## Deploy to production
	@chmod +x scripts/prod-deploy.sh
	@./scripts/prod-deploy.sh

prod-stop: ## Stop production environment
	@docker-compose -f docker-compose.prod.yml down

prod-logs: ## Show production logs
	@docker-compose -f docker-compose.prod.yml logs -f

prod-backup: ## Backup production database
	@mkdir -p backups
	@docker-compose -f docker-compose.prod.yml exec postgres pg_dump -U movieuser movieuserservice > backups/backup_$$(date +%Y%m%d_%H%M%S).sql
	@echo "Database backup created in backups/ directory"

# Build commands
build: ## Build Docker image
	@docker build -f docker/Dockerfile -t movie-user-service:latest .

build-no-cache: ## Build Docker image without cache
	@docker build --no-cache -f docker/Dockerfile -t movie-user-service:latest .

# Utility commands
clean: ## Clean Docker resources
	@docker system prune -f
	@docker volume prune -f

test: ## Run tests in container
	@docker-compose exec movie-user-api dotnet test

migrations-add: ## Add new migration (usage: make migrations-add NAME=MigrationName)
	@docker-compose exec movie-user-api dotnet ef migrations add $(NAME) --project /app/Movie.User.Service.Infra.dll

migrations-update: ## Update database with migrations
	@docker-compose exec movie-user-api dotnet ef database update

# Health checks
health: ## Check service health
	@echo "Checking API health..."
	@curl -f http://localhost:8000/health || echo "API is not healthy"
	@echo ""
	@echo "Checking database connection..."
	@docker-compose exec postgres pg_isready -U movieuser -d movieuserservice || echo "Database is not ready"