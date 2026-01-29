# Movie User Service - Infraestrutura

Esta camada contém toda a implementação de infraestrutura para o serviço de usuários, incluindo acesso a dados com PostgreSQL.

## Estrutura

```
Movie.User.Service.Infra/
├── Configuration/          # Configurações da infraestrutura
├── Data/                  # DbContext e configurações do EF Core
│   └── Configurations/    # Configurações das entidades
├── Migrations/            # Migrations do Entity Framework
├── Repositories/          # Implementações dos repositórios
└── Scripts/              # Scripts SQL para setup do banco
```

## Configuração do Banco de Dados

### 1. Instalação do PostgreSQL

Certifique-se de ter o PostgreSQL instalado e rodando em sua máquina.

### 2. Criação do Banco

Execute o script `Scripts/CreateDatabase.sql` como superusuário do PostgreSQL:

```bash
psql -U postgres -f Scripts/CreateDatabase.sql
```

### 3. String de Conexão

Adicione a string de conexão no `appsettings.json` da API:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=movieuserservice;Username=movieuser;Password=moviepass123"
  },
  "DatabaseSettings": {
    "CommandTimeout": 30,
    "EnableSensitiveDataLogging": false,
    "EnableDetailedErrors": false,
    "MaxRetryCount": 3,
    "MaxRetryDelay": "00:00:30"
  }
}
```

### 4. Aplicar Migrations

As migrations serão aplicadas automaticamente na inicialização da aplicação através do método `MigrateDatabaseAsync()`.

Para aplicar manualmente via CLI:

```bash
dotnet ef database update --project src/Movie.User.Service.Infra --startup-project src/Movie.User.Service.Api
```

### 5. Dados de Exemplo (Opcional)

Para inserir dados de exemplo, execute:

```bash
psql -U movieuser -d movieuserservice -f Scripts/SeedData.sql
```

## Uso na API

No `Program.cs` da API, registre os serviços:

```csharp
// Adicionar infraestrutura
builder.Services.AddInfrastructure(builder.Configuration);

// Após build da aplicação, aplicar migrations
var app = builder.Build();

// Aplicar migrations automaticamente
await app.Services.MigrateDatabaseAsync();
```

## Funcionalidades Implementadas

### DbContext
- `ApplicationDbContext`: Contexto principal do Entity Framework
- Configuração otimizada para PostgreSQL
- Suporte a migrations automáticas

### Repositórios
- `UserRepository`: Implementação completa do `IUserRepository`
- Operações CRUD completas
- Consultas otimizadas com índices
- Soft delete (desativação ao invés de exclusão física)

### Configurações
- Mapeamento completo da entidade `User`
- Configuração do Value Object `Address`
- Índices para performance
- Constraints de unicidade

### Migrations
- Migration inicial com estrutura completa
- Scripts SQL para setup do ambiente
- Dados de exemplo para testes

## Comandos Úteis

### Entity Framework CLI

```bash
# Adicionar nova migration
dotnet ef migrations add NomeDaMigration --project src/Movie.User.Service.Infra --startup-project src/Movie.User.Service.Api

# Aplicar migrations
dotnet ef database update --project src/Movie.User.Service.Infra --startup-project src/Movie.User.Service.Api

# Reverter migration
dotnet ef database update NomeMigrationAnterior --project src/Movie.User.Service.Infra --startup-project src/Movie.User.Service.Api

# Gerar script SQL
dotnet ef migrations script --project src/Movie.User.Service.Infra --startup-project src/Movie.User.Service.Api
```

### PostgreSQL

```bash
# Conectar ao banco
psql -U movieuser -d movieuserservice

# Listar tabelas
\dt

# Descrever estrutura da tabela
\d users

# Verificar dados
SELECT * FROM users;
```