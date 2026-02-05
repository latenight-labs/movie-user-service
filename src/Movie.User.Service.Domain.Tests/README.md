# Movie.User.Service.Domain.Tests

Domain layer unit tests for the Movie User Service.

**Framework**: .NET 9.0

## Test Structure

```
Movie.User.Service.Domain.Tests/
├── Configuration/
│   ├── LengthRuleTests.cs          # Tests for LengthRule validation rules
│   └── UserDomainOptionsTests.cs   # Tests for UserDomainOptions configuration
├── Entities/
│   └── UserTests.cs                # Tests for User entity business logic
└── ValueObjects/
    ├── AddressTests.cs             # Tests for Address value object
    └── LaunchDateTests.cs          # Tests for LaunchDate value object
```

## Test Coverage

### Entities
- **User**: Constructor validation, property updates, business operations (Update, UpdateLastLogin, Deactivate, Activate)

### Value Objects
- **Address**: Constructor validation, property requirements, equality comparison
- **LaunchDate**: Date comparison methods (IsAfter, Clamp)

### Configuration
- **LengthRule**: Property validation, constructor validation, Min/Max constraints
- **UserDomainOptions**: Configuration property access and initialization

## Running Tests

```bash
# Run all domain tests
dotnet test src/Movie.User.Service.Domain.Tests/

# Run with coverage
dotnet test src/Movie.User.Service.Domain.Tests/ --collect:"XPlat Code Coverage"

# Run specific test class
dotnet test src/Movie.User.Service.Domain.Tests/ --filter "FullyQualifiedName~UserTests"
```

## Test Framework

- **xUnit**: Testing framework
- **FluentAssertions**: Readable assertion library
- **Microsoft.NET.Test.Sdk**: Test platform

## Domain Testing Principles

1. **Unit Tests**: Each test focuses on a single behavior/unit
2. **Arrange-Act-Assert**: Clear test structure
3. **Descriptive Names**: Test names describe what they verify
4. **Edge Cases**: Tests cover boundary conditions and error scenarios
5. **Immutable Validation**: Value objects maintain immutability where appropriate