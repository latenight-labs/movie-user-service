# 🏗️ Arquitetura do Movie User Service

## 📋 Visão Geral

O **Movie User Service** é um microserviço desenvolvido em **.NET 9** que implementa **Clean Architecture** com **CQRS (Command Query Responsibility Segregation)** e **Domain-Driven Design (DDD)**. O serviço é responsável pelo gerenciamento completo de usuários em um sistema de filmes.

## 🎯 Objetivos Arquiteturais

- ✅ **Separação de Responsabilidades**: Cada camada tem uma responsabilidade específica
- ✅ **Testabilidade**: Código facilmente testável e mockável
- ✅ **Manutenibilidade**: Fácil de entender, modificar e estender
- ✅ **Escalabilidade**: Preparado para crescimento e mudanças
- ✅ **Performance**: Otimizado para operações de leitura e escrita
- ✅ **Flexibilidade**: Fácil adaptação a novos requisitos

## 🏛️ Clean Architecture

### Estrutura de Camadas

```
┌─────────────────────────────────────────────────────────────┐
│                    🌐 API Layer                              │
│  Controllers, Requests, Responses, Validators, Swagger      │
├─────────────────────────────────────────────────────────────┤
│                 🏗️ Application Layer                        │
│     Commands, Queries, Handlers, DTOs, Behaviors           │
├─────────────────────────────────────────────────────────────┤
│                  🎯 Domain Layer                            │
│        Entities, Value Objects, Repositories               │
├─────────────────────────────────────────────────────────────┤
│                🗄️ Infrastructure Layer                      │
│    Database, Repositories, External Services               │
└─────────────────────────────────────────────────────────────┘
```

### Dependências

```
API Layer ──────────┐
                    ├──► Application Layer ──► Domain Layer
Infrastructure ─────┘                              ▲
                                                   │
                                    (Implementa interfaces)
```

**Princípio**: As dependências sempre apontam para dentro (Domain é o centro)

## 📁 Estrutura de Pastas

```
src/
├── Movie.User.Service.Api/              # 🌐 API Layer
│   ├── Controllers/                     # Endpoints REST
│   ├── Requests/                        # DTOs de entrada
│   ├── Responses/                       # DTOs de saída
│   ├── Validators/                      # Validações da API
│   ├── Mappings/                        # Mapeamentos API ↔ Application
│   └── DependencyInjection.cs          # Configuração da camada
│
├── Movie.User.Service.Service/          # 🏗️ Application Layer
│   ├── Users/
│   │   ├── Commands/                    # Operações de escrita
│   │   ├── Queries/                     # Operações de leitura
│   │   ├── Handlers/                    # Lógica de negócio
│   │   ├── DTOs/                        # Objetos de transferência
│   │   ├── Validators/                  # Validações de negócio
│   │   ├── Mappings/                    # Mapeamentos Domain ↔ DTOs
│   │   └── SearchStrategies/            # Strategy Pattern para busca
│   ├── Common/                          # Utilitários compartilhados
│   └── DependencyInjection.cs          # Configuração da camada
│
├── Movie.Usar.Service.Domain/           # 🎯 Domain Layer
│   ├── Entities/                        # Entidades de domínio
│   ├── ValueObjects/                    # Objetos de valor
│   └── Repositories/                    # Interfaces dos repositórios
│
└── Movie.User.Service.Infra/            # 🗄️ Infrastructure Layer
    ├── Data/                            # Entity Framework
    │   └── Configurations/              # Configurações EF
    ├── Repositories/                    # Implementações dos repositórios
    ├── Migrations/                      # Migrações do banco
    ├── Scripts/                         # Scripts SQL
    └── DependencyInjection.cs          # Configuração da camada
```

## 🎨 Padrões de Design Implementados

### 1. 🔄 CQRS (Command Query Responsibility Segregation)

**Separação entre operações de leitura e escrita**

#### Commands (Escrita)
```csharp
// Operações que modificam estado
public record CreateUserCommand(CreateUserRequest Request) : IRequest<Result<UserDto>>;
public record UpdateUserCommand(int Id, UpdateUserRequest Request) : IRequest<Result<UserDto>>;
public record DeleteUserCommand(int Id) : IRequest<Result>;
```

#### Queries (Leitura)
```csharp
// Operações que apenas consultam dados
public record GetUserByIdQuery(int Id) : IRequest<Result<UserDto>>;
public record GetAllUsersQuery() : IRequest<Result<IEnumerable<UserDto>>>;
public record GetUsersByFilterQuery(...) : IRequest<Result<IEnumerable<UserDto>>>;
```

**Vantagens:**
- ✅ Separação clara de responsabilidades
- ✅ Otimizações específicas para leitura/escrita
- ✅ Escalabilidade independente
- ✅ Testabilidade aprimorada

### 2. 🎯 Mediator Pattern

**Centralização da comunicação entre componentes**

```csharp
[HttpPost]
public async Task<ActionResult<UserResponse>> CreateUser(CreateUserRequest request)
{
    var command = new CreateUserCommand(request);
    var result = await _mediator.Send(command); // ← Mediator
    
    return result.IsSuccess 
        ? Ok(result.Value.ToResponse()) 
        : BadRequest(result.Errors);
}
```

**Vantagens:**
- ✅ Baixo acoplamento entre componentes
- ✅ Fácil adição de cross-cutting concerns
- ✅ Pipeline de behaviors (validação, logging, cache)

### 3. 🏭 Repository Pattern

**Abstração do acesso a dados**

```csharp
// Interface no Domain
public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<User> AddAsync(User user, CancellationToken cancellationToken);
    // ... outros métodos
}

// Implementação na Infrastructure
public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;
    // ... implementação com EF Core
}
```

**Vantagens:**
- ✅ Testabilidade (fácil mock)
- ✅ Flexibilidade de implementação
- ✅ Separação entre domínio e persistência

### 4. 🎭 Strategy Pattern

**Diferentes estratégias de busca de usuários**

```csharp
public interface IUserSearchStrategy
{
    bool CanApply(GetUsersByFilterQuery query);
    Task<IEnumerable<User>> SearchAsync(GetUsersByFilterQuery query, ...);
}

// Implementações específicas
public class CitySearchStrategy : IUserSearchStrategy { ... }
public class StateSearchStrategy : IUserSearchStrategy { ... }
public class UsernameSearchStrategy : IUserSearchStrategy { ... }
```

**Handler que usa as estratégias:**
```csharp
public class GetUsersByFilterQueryHandler
{
    private readonly List<IUserSearchStrategy> _strategies;
    
    public async Task<Result<IEnumerable<UserDto>>> Handle(...)
    {
        var strategy = _strategies.FirstOrDefault(s => s.CanApply(request));
        return await strategy.SearchAsync(request, _repository, cancellationToken);
    }
}
```

**Vantagens:**
- ✅ Extensibilidade (fácil adicionar novas estratégias)
- ✅ Single Responsibility Principle
- ✅ Open/Closed Principle

### 5. 🏗️ Builder Pattern (Implicit)

**Construção de objetos complexos**

```csharp
// Value Object com validação
public class Address
{
    private Address() { } // EF Core
    
    public Address(string street, string city, string state, string zipCode, string country)
    {
        // Validações no construtor
        if (string.IsNullOrWhiteSpace(street))
            throw new ArgumentException("Street cannot be empty", nameof(street));
        // ... outras validações
        
        Street = street;
        City = city;
        // ... atribuições
    }
}
```

### 6. 🎪 Decorator Pattern (Pipeline Behaviors)

**Adição de funcionalidades transversais**

```csharp
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, ...)
    {
        // Validação antes da execução
        var validationResult = await _validator.ValidateAsync(request);
        
        if (!validationResult.IsValid)
            return CreateValidationError(validationResult.Errors);
            
        // Continua para o próximo behavior/handler
        return await next();
    }
}
```

**Pipeline de Behaviors:**
```
Request → ValidationBehavior → LoggingBehavior → Handler → Response
```

## 🎯 Domain-Driven Design (DDD)

### Entidades

```csharp
public class User
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Username { get; private set; }
    public string Email { get; private set; }
    public string Phone { get; private set; }
    public Address Address { get; private set; } // Value Object
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public bool IsActive { get; private set; }

    // Comportamentos de domínio
    public void Update(string name, string username, string email, string phone, Address address)
    {
        Name = name;
        Username = username;
        Email = email;
        Phone = phone;
        Address = address;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }
}
```

### Value Objects

```csharp
public class Address
{
    public string Street { get; private set; }
    public string City { get; private set; }
    public string State { get; private set; }
    public string ZipCode { get; private set; }
    public string Country { get; private set; }

    // Imutabilidade e validação
    public Address(string street, string city, string state, string zipCode, string country)
    {
        // Validações...
        Street = street;
        City = city;
        State = state;
        ZipCode = zipCode;
        Country = country;
    }

    // Equality por valor
    public override bool Equals(object? obj) { ... }
    public override int GetHashCode() { ... }
}
```

## 🗄️ Persistência de Dados

### Entity Framework Core

**Configuração da Entidade:**
```csharp
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        
        // Propriedades
        builder.Property(u => u.Name).HasMaxLength(200).IsRequired();
        builder.Property(u => u.Username).HasMaxLength(100).IsRequired();
        
        // Value Object como Owned Entity
        builder.OwnsOne(u => u.Address, address =>
        {
            address.Property(a => a.Street).HasColumnName("street").HasMaxLength(255);
            address.Property(a => a.City).HasColumnName("city").HasMaxLength(100);
            // ... outras propriedades
        });
        
        // Índices
        builder.HasIndex(u => u.Email).IsUnique();
        builder.HasIndex(u => u.Username).IsUnique();
    }
}
```

### PostgreSQL

**Características:**
- ✅ ACID compliance
- ✅ JSON support (futuro)
- ✅ Full-text search
- ✅ Extensibilidade
- ✅ Performance

## 🔧 Injeção de Dependência

### Organização por Camada

```csharp
// Program.cs - Orquestração
var builder = WebApplication.CreateBuilder(args);

// 🌐 API Layer
builder.Services.AddApiServices();

// 🏗️ Application Layer  
builder.Services.AddApplicationServices();

// 🗄️ Infrastructure Layer
builder.Services.AddInfrastructureServices(builder.Configuration);
```

### API Layer
```csharp
public static IServiceCollection AddApiServices(this IServiceCollection services)
{
    services.AddControllers();
    services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    services.AddSwaggerGen();
    services.AddHealthChecks();
    services.AddCors();
    return services;
}
```

### Application Layer
```csharp
public static IServiceCollection AddApplicationServices(this IServiceCollection services)
{
    services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    
    // Strategy Pattern
    services.AddScoped<IUserSearchStrategy, CitySearchStrategy>();
    services.AddScoped<IUserSearchStrategy, StateSearchStrategy>();
    // ... outras estratégias
    
    return services;
}
```

### Infrastructure Layer
```csharp
public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
{
    services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
    
    services.AddScoped<IUserRepository, UserRepository>();
    
    return services;
}
```

## 🛡️ Validação

### Múltiplas Camadas de Validação

#### 1. API Layer (FluentValidation)
```csharp
public class SearchUsersRequestValidator : AbstractValidator<SearchUsersRequest>
{
    public SearchUsersRequestValidator()
    {
        RuleFor(x => x).Must(HaveAtLeastOneFilter)
            .WithMessage("Pelo menos um filtro deve ser fornecido");
            
        When(x => !string.IsNullOrWhiteSpace(x.Username), () =>
        {
            RuleFor(x => x.Username).MinimumLength(3).MaximumLength(50);
        });
    }
}
```

#### 2. Application Layer (Pipeline Behavior)
```csharp
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    public async Task<TResponse> Handle(TRequest request, ...)
    {
        var validationResult = await _validator.ValidateAsync(request);
        
        if (!validationResult.IsValid)
            return CreateValidationError(validationResult.Errors);
            
        return await next();
    }
}
```

#### 3. Domain Layer (Business Rules)
```csharp
public class User
{
    public User(string name, string username, string email, string phone, Address address)
    {
        // Validações de domínio
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Name is required");
            
        Name = name;
        // ... outras atribuições
    }
}
```

## 📊 Tratamento de Erros

### Result Pattern

```csharp
public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public List<string> Errors { get; }
    
    public static Result Success() => new(true, new List<string>());
    public static Result Failure(string error) => new(false, new List<string> { error });
    public static Result Failure(List<string> errors) => new(false, errors);
}

public class Result<T> : Result
{
    public T Value { get; }
    
    public static Result<T> Success(T value) => new(true, value, new List<string>());
    public static Result<T> NotFound(string entity) => new(false, default, new List<string> { $"{entity} not found" });
}
```

**Uso nos Handlers:**
```csharp
public async Task<Result<UserDto>> Handle(GetUserByIdQuery request, ...)
{
    var user = await _repository.GetByIdAsync(request.Id);
    
    if (user == null)
        return Result<UserDto>.NotFound("User");
        
    return Result<UserDto>.Success(user.ToDto());
}
```

## 🔄 Mapeamentos

### Separação por Responsabilidade

#### API ↔ Application
```csharp
public static class ApiMappingProfile
{
    public static UserResponse ToResponse(this UserDto dto)
    {
        return new UserResponse
        {
            Id = dto.Id,
            Name = dto.Name,
            Username = dto.Username,
            // ... outros campos
        };
    }
}
```

#### Application ↔ Domain
```csharp
public static class UserMappingProfile
{
    public static UserDto ToDto(this User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Username = user.Username,
            // ... outros campos
        };
    }
}
```

## 🚀 Benefícios da Arquitetura

### 🧪 Testabilidade
- **Unit Tests**: Cada handler pode ser testado isoladamente
- **Integration Tests**: Testes de ponta a ponta com banco em memória
- **Mocking**: Interfaces facilitam criação de mocks

### 🔧 Manutenibilidade
- **Separação Clara**: Cada camada tem responsabilidade específica
- **Baixo Acoplamento**: Mudanças em uma camada não afetam outras
- **Alto Coesão**: Componentes relacionados ficam juntos

### 📈 Escalabilidade
- **CQRS**: Permite otimizações específicas para leitura/escrita
- **Strategy Pattern**: Fácil adição de novas funcionalidades
- **Microserviços**: Arquitetura preparada para distribuição

### 🎯 Flexibilidade
- **Repository Pattern**: Fácil troca de tecnologia de persistência
- **Mediator**: Fácil adição de cross-cutting concerns
- **DI**: Fácil substituição de implementações

## 🔮 Evoluções Futuras

### Possíveis Melhorias

1. **Event Sourcing**: Para auditoria completa
2. **CQRS com bancos separados**: Read/Write databases
3. **Cache distribuído**: Redis para performance
4. **Message Queues**: Para comunicação assíncrona
5. **Domain Events**: Para desacoplamento de side effects

### Preparação para Microserviços

A arquitetura atual já está preparada para:
- ✅ Separação em serviços independentes
- ✅ Comunicação via HTTP/gRPC
- ✅ Event-driven architecture
- ✅ Containerização (Docker)

## 📚 Conclusão

Esta arquitetura combina as melhores práticas de **Clean Architecture**, **DDD** e **CQRS** para criar um sistema:

- 🏗️ **Bem estruturado**: Separação clara de responsabilidades
- 🧪 **Testável**: Fácil criação de testes automatizados  
- 🔧 **Manutenível**: Código limpo e organizados
- 📈 **Escalável**: Preparado para crescimento
- 🎯 **Flexível**: Adaptável a mudanças de requisitos

A arquitetura serve como base sólida para o desenvolvimento de um sistema robusto e profissional de gerenciamento de usuários.