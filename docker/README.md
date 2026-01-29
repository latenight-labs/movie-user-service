# 🐳 Docker Configuration for Movie User Service

Este diretório contém toda a configuração Docker para o Movie User Service, seguindo as melhores práticas para aplicações .NET 8 com Clean Architecture.

## 📁 Estrutura de Arquivos

```
docker/
├── Dockerfile              # Multi-stage Dockerfile otimizado
├── nginx/
│   └── nginx.conf          # Configuração do Nginx para produção
└── README.md               # Esta documentação

scripts/
├── dev-start.sh            # Script para iniciar ambiente de desenvolvimento
├── dev-stop.sh             # Script para parar ambiente de desenvolvimento
└── prod-deploy.sh          # Script para deploy em produção

docker-compose.yml          # Ambiente de desenvolvimento
docker-compose.prod.yml     # Ambiente de produção
.dockerignore              # Arquivos ignorados no build
.env.example               # Exemplo de variáveis de ambiente
Makefile                   # Comandos automatizados
```

## 🚀 Início Rápido

### Desenvolvimento

```bash
# Usando Makefile (recomendado)
make dev-start

# Ou usando scripts diretamente
./scripts/dev-start.sh

# Ou usando docker-compose
docker-compose up --build -d
```

### Produção

```bash
# Configurar variáveis de ambiente
cp .env.example .env
# Editar .env com valores de produção

# Deploy
make prod-deploy

# Ou usando script
./scripts/prod-deploy.sh
```

## 🔧 Comandos Úteis

### Desenvolvimento
```bash
make dev-start      # Iniciar ambiente
make dev-stop       # Parar ambiente
make dev-logs       # Ver logs
make dev-restart    # Reiniciar API
make dev-shell      # Acessar container da API
make dev-db         # Acessar PostgreSQL
make dev-reset      # Reset completo (remove volumes)
```

### Produção
```bash
make prod-deploy    # Deploy para produção
make prod-stop      # Parar produção
make prod-logs      # Ver logs de produção
make prod-backup    # Backup do banco
```

### Utilitários
```bash
make build          # Build da imagem
make clean          # Limpar recursos Docker
make test           # Executar testes
make health         # Verificar saúde dos serviços
```

## 🏗️ Arquitetura Docker

### Multi-stage Dockerfile

O Dockerfile utiliza múltiplos estágios para otimização:

1. **base**: Runtime ASP.NET Core Alpine
2. **build**: SDK .NET 8 para compilação
3. **publish**: Publicação otimizada
4. **final**: Imagem final mínima

### Características:

- ✅ **Imagem Alpine** para menor tamanho
- ✅ **Usuário não-root** para segurança
- ✅ **Multi-stage build** para otimização
- ✅ **Health checks** integrados
- ✅ **Layer caching** otimizado
- ✅ **Minimal runtime** dependencies

## 🗄️ Banco de Dados

### PostgreSQL 16 Alpine
- **Porta**: 5432
- **Database**: movieuserservice
- **User**: movieuser
- **Password**: Configurável via .env

### pgAdmin (Desenvolvimento)
- **URL**: http://localhost:8080
- **Email**: admin@movieuser.com
- **Password**: admin123

## 🌐 Nginx (Produção)

### Características:
- ✅ **Reverse proxy** para a API
- ✅ **Rate limiting** (10 req/s)
- ✅ **Gzip compression**
- ✅ **Security headers**
- ✅ **SSL ready** (configuração comentada)
- ✅ **Health check** endpoint

### Endpoints:
- **API**: http://localhost/api/*
- **Health**: http://localhost/health
- **Swagger**: http://localhost/swagger (dev only)

## 🔒 Segurança

### Implementadas:
- ✅ Usuário não-root nos containers
- ✅ Security headers no Nginx
- ✅ Rate limiting
- ✅ Secrets via environment variables
- ✅ Network isolation
- ✅ Resource limits

### Para Produção:
- 🔧 Configure SSL/TLS no Nginx
- 🔧 Use secrets management (Docker Secrets, Kubernetes Secrets)
- 🔧 Configure firewall rules
- 🔧 Enable container scanning
- 🔧 Use private registry

## 📊 Monitoramento

### Health Checks:
- **API**: `GET /health`
- **Database**: `pg_isready`
- **Containers**: Docker health checks

### Logs:
```bash
# Ver logs em tempo real
make dev-logs

# Logs específicos
docker-compose logs -f movie-user-api
docker-compose logs -f postgres
```

## 🔄 CI/CD Integration

### GitHub Actions Example:
```yaml
- name: Build and Push Docker Image
  run: |
    docker build -f docker/Dockerfile -t movie-user-service:${{ github.sha }} .
    docker push movie-user-service:${{ github.sha }}

- name: Deploy to Production
  run: |
    docker-compose -f docker-compose.prod.yml up -d
```

## 🐛 Troubleshooting

### Problemas Comuns:

1. **Port already in use**:
   ```bash
   # Verificar portas em uso
   netstat -tulpn | grep :5432
   # Parar containers conflitantes
   docker-compose down
   ```

2. **Database connection failed**:
   ```bash
   # Verificar saúde do PostgreSQL
   docker-compose exec postgres pg_isready -U movieuser
   # Ver logs do banco
   docker-compose logs postgres
   ```

3. **API not responding**:
   ```bash
   # Verificar logs da API
   docker-compose logs movie-user-api
   # Verificar health check
   curl http://localhost:8000/health
   ```

4. **Build failures**:
   ```bash
   # Build sem cache
   make build-no-cache
   # Limpar recursos Docker
   make clean
   ```

## 📈 Performance

### Otimizações Implementadas:
- ✅ Layer caching no Dockerfile
- ✅ Multi-stage build
- ✅ Alpine Linux (imagens menores)
- ✅ Connection pooling no PostgreSQL
- ✅ Gzip compression no Nginx
- ✅ Resource limits nos containers

### Métricas Típicas:
- **Imagem final**: ~200MB
- **Startup time**: ~10-15s
- **Memory usage**: ~100-200MB
- **Build time**: ~2-3min

## 🔗 Links Úteis

- [Docker Best Practices](https://docs.docker.com/develop/dev-best-practices/)
- [.NET Docker Images](https://hub.docker.com/_/microsoft-dotnet)
- [PostgreSQL Docker](https://hub.docker.com/_/postgres)
- [Nginx Docker](https://hub.docker.com/_/nginx)