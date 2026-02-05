using FluentAssertions;

namespace Movie.User.Service.Domain.Tests.Entities;

public class UserTests
{
    private readonly Address _validAddress = new("123 Main St", "Anytown", "State", "12345-678", "Country");

    [Fact]
    public void Constructor_ShouldCreateUserWithValidData()
    {
        // Arrange
        var name = "John Doe";
        var username = "johndoe";
        var email = "john@example.com";
        var phone = "+1234567890";

        // Act
        var user = new Domain.Entities.User(name, username, email, phone, _validAddress);

        // Assert
        user.Name.Should().Be(name);
        user.Username.Should().Be(username);
        user.Email.Should().Be(email);
        user.Phone.Should().Be(phone);
        user.Address.Should().Be(_validAddress);
        user.IsActive.Should().BeTrue();
        user.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        user.UpdatedAt.Should().BeNull();
        user.LastLoginAt.Should().BeNull();
    }

    [Fact]
    public void Constructor_ShouldSetIdToDefault()
    {
        // Arrange & Act
        var user = new Domain.Entities.User("John", "john", "john@example.com", "+1234567890", _validAddress);

        // Assert
        user.Id.Should().Be(0); // Default value for int
    }

    [Fact]
    public void Update_ShouldModifyUserDataAndSetUpdatedAt()
    {
        // Arrange
        var user = new Domain.Entities.User("John", "john", "john@example.com", "+1234567890", _validAddress);
        var originalCreatedAt = user.CreatedAt;

        var newName = "Jane Doe";
        var newUsername = "janedoe";
        var newEmail = "jane@example.com";
        var newPhone = "+0987654321";
        var newAddress = new Address("456 Oak St", "Newtown", "NewState", "54321-987", "NewCountry");

        // Act
        user.Update(newName, newUsername, newEmail, newPhone, newAddress);

        // Assert
        user.Name.Should().Be(newName);
        user.Username.Should().Be(newUsername);
        user.Email.Should().Be(newEmail);
        user.Phone.Should().Be(newPhone);
        user.Address.Should().Be(newAddress);
        user.CreatedAt.Should().Be(originalCreatedAt); // Should not change
        user.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        user.IsActive.Should().BeTrue(); // Should not change
    }

    [Fact]
    public void UpdateLastLogin_ShouldSetLastLoginAtAndUpdatedAt()
    {
        // Arrange
        var user = new Domain.Entities.User("John", "john", "john@example.com", "+1234567890", _validAddress);
        var originalCreatedAt = user.CreatedAt;

        // Act
        user.UpdateLastLogin();

        // Assert
        user.LastLoginAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        user.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        user.CreatedAt.Should().Be(originalCreatedAt); // Should not change
        user.IsActive.Should().BeTrue(); // Should not change
    }

    [Fact]
    public void Deactivate_ShouldSetIsActiveToFalseAndUpdateUpdatedAt()
    {
        // Arrange
        var user = new Domain.Entities.User("John", "john", "john@example.com", "+1234567890", _validAddress);
        var originalCreatedAt = user.CreatedAt;

        // Act
        user.Deactivate();

        // Assert
        user.IsActive.Should().BeFalse();
        user.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        user.CreatedAt.Should().Be(originalCreatedAt); // Should not change
    }

    [Fact]
    public void Activate_ShouldSetIsActiveToTrueAndUpdateUpdatedAt()
    {
        // Arrange
        var user = new Domain.Entities.User("John", "john", "john@example.com", "+1234567890", _validAddress);
        user.Deactivate(); // First deactivate
        var originalCreatedAt = user.CreatedAt;

        // Act
        user.Activate();

        // Assert
        user.IsActive.Should().BeTrue();
        user.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        user.CreatedAt.Should().Be(originalCreatedAt); // Should not change
    }

    [Fact]
    public void MultipleOperations_ShouldMaintainCorrectTimestamps()
    {
        // Arrange
        var user = new Domain.Entities.User("John", "john", "john@example.com", "+1234567890", _validAddress);
        var createdAt = user.CreatedAt;

        // Act - Perform multiple operations
        Thread.Sleep(10); // Small delay to ensure different timestamps
        user.UpdateLastLogin();
        var lastLoginAt1 = user.LastLoginAt;
        var updatedAt1 = user.UpdatedAt;

        Thread.Sleep(10);
        user.Update("Jane", "jane", "jane@example.com", "+0987654321", _validAddress);
        var updatedAt2 = user.UpdatedAt;

        // Assert
        user.CreatedAt.Should().Be(createdAt);
        lastLoginAt1.Should().NotBeNull();
        updatedAt1.Should().NotBeNull();
        if (lastLoginAt1.HasValue && updatedAt1.HasValue)
        {
            lastLoginAt1.Value.Should().BeCloseTo(updatedAt1.Value, TimeSpan.FromMilliseconds(1));
        }
        updatedAt2.Should().NotBeNull();
        if (updatedAt1.HasValue && updatedAt2.HasValue)
        {
            updatedAt2.Value.Should().BeAfter(updatedAt1.Value);
        }
        user.Name.Should().Be("Jane");
        user.LastLoginAt.Should().Be(lastLoginAt1);
    }
}