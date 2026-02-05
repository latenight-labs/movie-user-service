using FluentAssertions;

namespace Movie.User.Service.Domain.Tests.Configuration;

public class UserDomainOptionsTests
{
    [Fact]
    public void Constructor_ShouldInitializeAllPropertiesWithDefaultValues()
    {
        // Act
        var options = new UserDomainOptions();

        // Assert
        options.Name.Should().NotBeNull();
        options.Username.Should().NotBeNull();
        options.Email.Should().NotBeNull();
        options.Street.Should().NotBeNull();
        options.City.Should().NotBeNull();
        options.State.Should().NotBeNull();
        options.Country.Should().NotBeNull();
        options.LaunchDate.Should().Be(default(DateTime));
    }

    [Fact]
    public void Properties_ShouldBeSettableWithObjectInitializer()
    {
        // Arrange
        var nameRule = new LengthRule { Max = 100, Min = 2 };
        var usernameRule = new LengthRule { Max = 50, Min = 3 };
        var launchDate = new DateTime(2024, 1, 1);
        var phoneRegex = @"^\+?[1-9]\d{1,14}$";
        var zipCodeRegex = @"^\d{5}-?\d{3}$";

        // Act
        var options = new UserDomainOptions
        {
            Name = nameRule,
            Username = usernameRule,
            LaunchDate = launchDate,
            PhoneRegex = phoneRegex,
            ZipCodeRegex = zipCodeRegex
        };

        // Assert
        options.Name.Should().Be(nameRule);
        options.Username.Should().Be(usernameRule);
        options.LaunchDate.Should().Be(launchDate);
        options.PhoneRegex.Should().Be(phoneRegex);
        options.ZipCodeRegex.Should().Be(zipCodeRegex);
    }

    [Fact]
    public void AllLengthRuleProperties_ShouldBeConfigurable()
    {
        // Arrange
        var customName = new LengthRule { Max = 200, Min = 1 };
        var customUsername = new LengthRule { Max = 30, Min = 2 };
        var customEmail = new LengthRule { Max = 150, Min = 5 };
        var customStreet = new LengthRule { Max = 100, Min = 5 };
        var customCity = new LengthRule { Max = 80, Min = 2 };
        var customState = new LengthRule { Max = 50, Min = 2 };
        var customCountry = new LengthRule { Max = 60, Min = 2 };

        // Act
        var options = new UserDomainOptions
        {
            Name = customName,
            Username = customUsername,
            Email = customEmail,
            Street = customStreet,
            City = customCity,
            State = customState,
            Country = customCountry
        };

        // Assert
        options.Name.Should().Be(customName);
        options.Username.Should().Be(customUsername);
        options.Email.Should().Be(customEmail);
        options.Street.Should().Be(customStreet);
        options.City.Should().Be(customCity);
        options.State.Should().Be(customState);
        options.Country.Should().Be(customCountry);
    }

    [Fact]
    public void RegexProperties_ShouldBeConfigurable()
    {
        // Arrange
        var phoneRegex = @"^\+[1-9]\d{1,14}$";
        var zipCodeRegex = @"^\d{8}$";

        // Act
        var options = new UserDomainOptions
        {
            PhoneRegex = phoneRegex,
            ZipCodeRegex = zipCodeRegex
        };

        // Assert
        options.PhoneRegex.Should().Be(phoneRegex);
        options.ZipCodeRegex.Should().Be(zipCodeRegex);
    }

    [Fact]
    public void LaunchDate_ShouldBeConfigurable()
    {
        // Arrange
        var launchDate = new DateTime(2026, 1, 1);

        // Act
        var options = new UserDomainOptions
        {
            LaunchDate = launchDate
        };

        // Assert
        options.LaunchDate.Should().Be(launchDate);
    }



    [Fact]
    public void CanCreateMultipleInstances_WithDifferentConfigurations()
    {
        // Act
        var strictOptions = new UserDomainOptions
        {
            Name = new LengthRule { Max = 50, Min = 5 },
            Username = new LengthRule { Max = 20, Min = 5 }
        };

        var lenientOptions = new UserDomainOptions
        {
            Name = new LengthRule { Max = 200, Min = 1 },
            Username = new LengthRule { Max = 100, Min = 3 }
        };

        // Assert
        strictOptions.Name.Min.Should().Be(5);
        strictOptions.Name.Max.Should().Be(50);
        lenientOptions.Name.Min.Should().Be(1);
        lenientOptions.Name.Max.Should().Be(200);
    }
}