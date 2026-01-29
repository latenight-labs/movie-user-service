# ✅ Validação com FluentValidation - Implementação

## 📋 O que foi implementado

### 1. **Validador para SearchUsersRequest** (API Layer)
- **Arquivo**: `Movie.User.Service.Api/Validators/SearchUsersRequestValidator.cs`
- **Validações**:
  - ✅ Pelo menos um filtro deve ser fornecido
  - ✅ Username: 3-50 caracteres (quando fornecido)
  - ✅ Phone: formato internacional válido (quando fornecido)
  - ✅ ZipCode: formato CEP válido (quando fornecido)
  - ✅ City, State, Country: 2-100 caracteres (quando fornecido)
  - ✅ Address: 5-200 caracteres (quando fornecido)

### 2. **Validador para GetUsersByFilterQuery** (Service Layer)
- **Arquivo**: `Movie.User.Service.Service/Users/Validators/GetUsersByFilterQueryValidator.cs`
- **Validações**: Mesmas do SearchUsersRequest
- **Execução**: Automática via `ValidationBehavior` quando passa pelo MediatR

### 3. **Validação na Controller**
- ✅ Validação manual do `SearchUsersRequest` antes de criar o Query
- ✅ Retorna erro HTTP 400 imediatamente se inválido
- ✅ Logging de erros de validação

---

## 🔄 Fluxo de Validação

### Para SearchUsers:

```
HTTP Request (SearchUsersRequest)
    ↓
Controller.ValidateAsync() ← Validação manual na API
    ↓ (se válido)
GetUsersByFilterQuery
    ↓
ValidationBehavior ← Validação automática no MediatR
    ↓ (se válido)
Handler
```

### Para CreateUser/UpdateUser:

```
HTTP Request (CreateUserRequest/UpdateUserRequest)
    ↓
CreateUserCommand/UpdateUserCommand
    ↓
ValidationBehavior ← Validação automática no MediatR
    ↓ (se válido)
Handler
```

---

## ✅ Validações Automáticas (via ValidationBehavior)

As seguintes validações são executadas **automaticamente** pelo `ValidationBehavior`:

1. ✅ `CreateUserCommand` → `CreateUserRequestValidator`
2. ✅ `UpdateUserCommand` → `UpdateUserRequestValidator`
3. ✅ `GetUsersByFilterQuery` → `GetUsersByFilterQueryValidator`

**Não é necessário** adicionar validação manual na controller para esses casos!

---

## 🧹 Validações Removidas/Desnecessárias

### ❌ Antes (se existissem):
```csharp
// Validação manual desnecessária
if (string.IsNullOrEmpty(request.Username))
    return BadRequest("Username é obrigatório");
```

### ✅ Agora:
- ✅ FluentValidation faz tudo automaticamente
- ✅ Controller apenas trata o resultado
- ✅ Código mais limpo e consistente

---

## 📝 Exemplo de Resposta de Erro

### Quando validação falha:

**Request:**
```http
GET /api/user/search
```

**Response (400 Bad Request):**
```json
{
  "errors": [
    "Pelo menos um filtro deve ser fornecido para a busca.",
    "Nome de usuário deve ter no mínimo 3 caracteres."
  ]
}
```

---

## 🎯 Benefícios

1. ✅ **Consistência**: Todas as validações seguem o mesmo padrão
2. ✅ **Reutilização**: Validators podem ser reutilizados
3. ✅ **Testabilidade**: Fácil testar validators isoladamente
4. ✅ **Manutenibilidade**: Regras de validação centralizadas
5. ✅ **Documentação**: Swagger mostra automaticamente as validações
6. ✅ **Separação de Responsabilidades**: Controller não faz validação manual

---

## 🔧 Configuração no Program.cs

```csharp
// FluentValidation Configuration
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateUserRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<GetUsersByFilterQueryValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<SearchUsersRequestValidator>();

// Pipeline Behaviors (validação automática)
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
```

---

## ✅ Checklist de Validação

- [x] SearchUsersRequest tem validador
- [x] GetUsersByFilterQuery tem validador
- [x] CreateUserRequest tem validador (já existia)
- [x] UpdateUserRequest tem validador (já existia)
- [x] Validação automática configurada no ValidationBehavior
- [x] Validação manual na controller apenas onde necessário (SearchUsers)
- [x] Todos os validators registrados no DI
- [x] Logging de erros de validação

---

## 🚀 Próximos Passos (Opcional)

1. **Validação de Email**: Adicionar validação de formato de email no GetUserByEmail
2. **Validação de ID**: Adicionar validação de ID positivo nos endpoints que recebem ID
3. **Custom Validators**: Criar validators customizados para regras de negócio complexas
4. **Localização**: Adicionar mensagens de erro localizadas
