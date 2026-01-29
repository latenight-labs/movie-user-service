# 🐳 Docker Setup - Movie User Service

## 📋 Estrutura Criada

```
movie-user-service/
├── docker/
│   ├── Dockerfile                 # Multi-stage Dockerfile otimizado
│   ├── nginx/
│   │   └── nginx.conf            # Configuração Nginx para produção
│   └── README.md                 # Documentação detalhada
├── scripts/
│   ├── dev-start.sh              # Script para desenvolvimento
│   ├── dev-stop.sh               # Script para parar desenvolvimento
│   └── prod-deploy.sh            # Script para produção
├── docker-compose.yml            # Ambiente de desenvolvimento
├── docker-compose.prod.yml       # Ambiente de produção
├── .dockerignore                 # Otimização de build
├── .env.example                  # Template de variáveis
├── Makefile                      # Comandos automatizados
└── DOCKER_SETUP.md              # Este arquivo
```

## 🚀 Como Usar

### 1. Preparação Inicial

```bash
# Copiar arquivo de ambiente
cp .env.example .env

# Tornar scripts executáveis (Linux/Mac)
chmod +x scripts/*.sh

# No Windows, use Git Bash ou WSL para executar os scripts
```

### 2. Desenvolvimento

```bash
# Opção 1: Usando Makefile (recomendado)
make dev-start

# Opção 2: Usando scripts
./scripts/dev-start.sh

# Opção 3: Docker Compose direto
docker-compose up --build -d
```

**Serviços disponíveis:**
- 🌐 **API**: http://localhost:8000
- 📚 **Swagger**: http://localhost:8000/swagger
- 🗄️ **PostgreSQL**: localhost:5432
- 🔧 **pgAdmin**: http://localhost:8080

### 3. Comandos Úteis

```bash
# Ver todos os comandos disponíveis
make help

# Logs em tempo real
make dev-logs

# Reiniciar apenas a API
make dev-restart

# Acessar shell da API
make dev-shell

# Acessar PostgreSQL
make dev-db

# Parar ambiente
make dev-stop

# Reset completo (remove dados)
make dev-reset
```

### 4. Produção

```bash
# Configurar variáveis de produção no .env
POSTGRES_PASSWORD=sua_senha_segura_aqui

# Deploy
make prod-deploy

# Backup do banco
make prod-backup

# Parar produção
make prod-stop
```

## 🔧 Características Técnicas

### Dockerfile Multi-stage
- ✅ **Base**: ASP.NET Core 9.0 Alpine
- ✅ **Build**: SDK .NET 9.0 Alpine
- ✅ **Segurança**: Usuário não-root
- ✅ **Otimização**: Layer caching
- ✅ **Health checks**: Integrados

### PostgreSQL
- ✅ **Versão**: 16 Alpine
- ✅ **Persistência**: Volumes Docker
- ✅ **Health checks**: pg_isready
- ✅ **Scripts**: Inicialização automática

### Nginx (Produção)
- ✅ **Reverse proxy**: Para a API
- ✅ **Rate limiting**: 10 req/s
- ✅ **Compressão**: Gzip habilitado
- ✅ **Segurança**: Headers de segurança
- ✅ **SSL Ready**: Configuração preparada

## 🔒 Segurança

### Implementado:
- ✅ Containers com usuário não-root
- ✅ Network isolation
- ✅ Security headers no Nginx
- ✅ Rate limiting
- ✅ Resource limits

### Para Produção:
- 🔧 Configure SSL/TLS
- 🔧 Use secrets management
- 🔧 Configure firewall
- 🔧 Habilite container scanning

## 📊 Monitoramento

### Health Checks:
```bash
# Verificar saúde dos serviços
make health

# Verificar status dos containers
docker-compose ps

# Logs específicos
docker-compose logs -f movie-user-api
```

### Métricas:
- **Imagem final**: ~200MB
- **Startup**: ~10-15s
- **Memory**: ~100-200MB
- **Build time**: ~2-3min

## 🐛 Troubleshooting

### Problemas Comuns:

1. **Porta em uso**:
   ```bash
   # Windows
   netstat -ano | findstr :5432
   # Linux/Mac
   netstat -tulpn | grep :5432
   ```

2. **Erro de conexão com banco**:
   ```bash
   # Verificar logs do PostgreSQL
   docker-compose logs postgres
   
   # Testar conexão
   docker-compose exec postgres pg_isready -U movieuser
   ```

3. **API não responde**:
   ```bash
   # Ver logs da API
   docker-compose logs movie-user-api
   
   # Testar health check
   curl http://localhost:8000/health
   ```

4. **Build falha**:
   ```bash
   # Build sem cache
   make build-no-cache
   
   # Limpar recursos
   make clean
   ```

## 🔄 CI/CD

### GitHub Actions Example:
```yaml
name: Docker Build and Deploy

on:
  push:
    branches: [main]

jobs:
  build:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      
      - name: Build Docker Image
        run: |
          docker build -f docker/Dockerfile -t movie-user-service:${{ github.sha }} .
          
      - name: Run Tests
        run: |
          docker-compose up -d postgres
          docker-compose run --rm movie-user-api dotnet test
          
      - name: Deploy to Production
        if: github.ref == 'refs/heads/main'
        run: |
          docker-compose -f docker-compose.prod.yml up -d
```

## 📚 Próximos Passos

1. **Configurar SSL** no Nginx para produção
2. **Implementar logging** estruturado (Serilog)
3. **Adicionar métricas** (Prometheus/Grafana)
4. **Configurar backup** automático do banco
5. **Implementar secrets** management
6. **Adicionar testes** de integração

## 🆘 Suporte

Para mais detalhes, consulte:
- 📖 `docker/README.md` - Documentação completa
- 🔧 `Makefile` - Todos os comandos disponíveis
- 🐳 [Docker Best Practices](https://docs.docker.com/develop/dev-best-practices/)

---

**Ambiente Docker configurado com sucesso! 🎉**

Use `make help` para ver todos os comandos disponíveis.