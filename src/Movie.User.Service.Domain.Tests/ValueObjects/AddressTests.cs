using FluentAssertions;

namespace Movie.User.Service.Domain.Tests.ValueObjects;

public class AddressTests
{
    [Fact]
    public void Constructor_ShouldCreateAddressWithValidData()
    {
        // Arrange
        var street = "123 Main St";
        var city = "Anytown";
        var state = "State";
        var zipCode = "12345-678";
        var country = "Country";

        // Act
        var address = new Address(street, city, state, zipCode, country);

        // Assert
        address.Street.Should().Be(street);
        address.City.Should().Be(city);
        address.State.Should().Be(state);
        address.ZipCode.Should().Be(zipCode);
        address.Country.Should().Be(country);
    }

    [Theory]
    [InlineData("", "Anytown", "State", "12345-678", "Country", nameof(Address.Street))]
    [InlineData("   ", "Anytown", "State", "12345-678", "Country", nameof(Address.Street))]
    [InlineData("123 Main St", "", "State", "12345-678", "Country", nameof(Address.City))]
    [InlineData("123 Main St", "   ", "State", "12345-678", "Country", nameof(Address.City))]
    [InlineData("123 Main St", "Anytown", "", "12345-678", "Country", nameof(Address.State))]
    [InlineData("123 Main St", "Anytown", "   ", "12345-678", "Country", nameof(Address.State))]
    [InlineData("123 Main St", "Anytown", "State", "", "Country", nameof(Address.ZipCode))]
    [InlineData("123 Main St", "Anytown", "State", "   ", "Country", nameof(Address.ZipCode))]
    [InlineData("123 Main St", "Anytown", "State", "12345-678", "", nameof(Address.Country))]
    [InlineData("123 Main St", "Anytown", "State", "12345-678", "   ", nameof(Address.Country))]
    public void Constructor_ShouldThrowArgumentException_WhenParameterIsNullOrEmpty(string street, string city, string state, string zipCode, string country, string expectedParamName)
    {
        // Act
        var action = () => new Address(street, city, state, zipCode, country);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage($"*{expectedParamName}*")
            .Where(e => e.Message.Contains("cannot be empty"));
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenStreetIsNull()
    {
        // Act
        var action = () => new Address(null!, "Anytown", "State", "12345-678", "Country");

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage($"*{nameof(Address.Street)}*")
            .Where(e => e.Message.Contains("cannot be empty"));
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenCityIsNull()
    {
        // Act
        var action = () => new Address("123 Main St", null!, "State", "12345-678", "Country");

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage($"*{nameof(Address.City)}*")
            .Where(e => e.Message.Contains("cannot be empty"));
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenStateIsNull()
    {
        // Act
        var action = () => new Address("123 Main St", "Anytown", null!, "12345-678", "Country");

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage($"*{nameof(Address.State)}*")
            .Where(e => e.Message.Contains("cannot be empty"));
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenZipCodeIsNull()
    {
        // Act
        var action = () => new Address("123 Main St", "Anytown", "State", null!, "Country");

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage($"*{nameof(Address.ZipCode)}*")
            .Where(e => e.Message.Contains("cannot be empty"));
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenCountryIsNull()
    {
        // Act
        var action = () => new Address("123 Main St", "Anytown", "State", "12345-678", null!);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage($"*{nameof(Address.Country)}*")
            .Where(e => e.Message.Contains("cannot be empty"));
    }

    [Fact]
    public void Equals_ShouldReturnTrue_ForSameAddressData()
    {
        // Arrange
        var address1 = new Address("123 Main St", "Anytown", "State", "12345-678", "Country");
        var address2 = new Address("123 Main St", "Anytown", "State", "12345-678", "Country");

        // Act & Assert
        address1.Equals(address2).Should().BeTrue();
        (address1 == address2).Should().BeTrue();
    }

    [Fact]
    public void Equals_ShouldReturnFalse_ForDifferentAddressData()
    {
        // Arrange
        var address1 = new Address("123 Main St", "Anytown", "State", "12345-678", "Country");
        var address2 = new Address("456 Oak St", "Anytown", "State", "12345-678", "Country");

        // Act & Assert
        address1.Equals(address2).Should().BeFalse();
        (address1 != address2).Should().BeTrue();
    }

    [Fact]
    public void Equals_ShouldReturnFalse_ForDifferentStreet()
    {
        // Arrange
        var address1 = new Address("123 Main St", "Anytown", "State", "12345-678", "Country");
        var address2 = new Address("456 Oak St", "Anytown", "State", "12345-678", "Country");

        // Act & Assert
        address1.Equals(address2).Should().BeFalse();
    }

    [Fact]
    public void Equals_ShouldReturnFalse_ForDifferentCity()
    {
        // Arrange
        var address1 = new Address("123 Main St", "Anytown", "State", "12345-678", "Country");
        var address2 = new Address("123 Main St", "Newtown", "State", "12345-678", "Country");

        // Act & Assert
        address1.Equals(address2).Should().BeFalse();
    }

    [Fact]
    public void Equals_ShouldReturnFalse_ForDifferentState()
    {
        // Arrange
        var address1 = new Address("123 Main St", "Anytown", "State", "12345-678", "Country");
        var address2 = new Address("123 Main St", "Anytown", "NewState", "12345-678", "Country");

        // Act & Assert
        address1.Equals(address2).Should().BeFalse();
    }

    [Fact]
    public void Equals_ShouldReturnFalse_ForDifferentZipCode()
    {
        // Arrange
        var address1 = new Address("123 Main St", "Anytown", "State", "12345-678", "Country");
        var address2 = new Address("123 Main St", "Anytown", "State", "98765-432", "Country");

        // Act & Assert
        address1.Equals(address2).Should().BeFalse();
    }

    [Fact]
    public void Equals_ShouldReturnFalse_ForDifferentCountry()
    {
        // Arrange
        var address1 = new Address("123 Main St", "Anytown", "State", "12345-678", "Country");
        var address2 = new Address("123 Main St", "Anytown", "State", "12345-678", "NewCountry");

        // Act & Assert
        address1.Equals(address2).Should().BeFalse();
    }

    [Fact]
    public void Equals_ShouldReturnFalse_WhenComparedWithNull()
    {
        // Arrange
        var address = new Address("123 Main St", "Anytown", "State", "12345-678", "Country");

        // Act & Assert
        address.Equals(null).Should().BeFalse();
    }

    [Fact]
    public void Equals_ShouldReturnFalse_WhenComparedWithDifferentType()
    {
        // Arrange
        var address = new Address("123 Main St", "Anytown", "State", "12345-678", "Country");

        // Act & Assert
        address.Equals("not an address").Should().BeFalse();
    }

    [Fact]
    public void GetHashCode_ShouldReturnSameValue_ForEqualAddresses()
    {
        // Arrange
        var address1 = new Address("123 Main St", "Anytown", "State", "12345-678", "Country");
        var address2 = new Address("123 Main St", "Anytown", "State", "12345-678", "Country");

        // Act & Assert
        address1.GetHashCode().Should().Be(address2.GetHashCode());
    }

    [Fact]
    public void GetHashCode_ShouldReturnDifferentValues_ForDifferentAddresses()
    {
        // Arrange
        var address1 = new Address("123 Main St", "Anytown", "State", "12345-678", "Country");
        var address2 = new Address("456 Oak St", "Anytown", "State", "12345-678", "Country");

        // Act & Assert
        address1.GetHashCode().Should().NotBe(address2.GetHashCode());
    }
}