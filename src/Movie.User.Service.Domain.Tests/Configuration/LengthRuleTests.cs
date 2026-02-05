using FluentAssertions;

namespace Movie.User.Service.Domain.Tests.Configuration;

public class LengthRuleTests
{
    [Fact]
    public void DefaultConstructor_ShouldCreateLengthRuleWithDefaultValues()
    {
        // Act
        var lengthRule = new LengthRule();

        // Assert
        lengthRule.Min.Should().Be(0);
        lengthRule.Max.Should().Be(0);
    }

    [Fact]
    public void ParameterizedConstructor_ShouldCreateLengthRuleWithValidValues()
    {
        // Arrange
        var min = 3;
        var max = 50;

        // Act
        var lengthRule = new LengthRule(min, max);

        // Assert
        lengthRule.Min.Should().Be(min);
        lengthRule.Max.Should().Be(max);
    }

    [Fact]
    public void ParameterizedConstructor_ShouldThrowArgumentException_WhenMinGreaterThanOrEqualToMax()
    {
        // Act & Assert - Min greater than Max
        var action1 = () => new LengthRule(10, 5);
        action1.Should().Throw<ArgumentException>()
            .WithMessage("Min must be less than Max");

        // Act & Assert - Min equal to Max
        var action2 = () => new LengthRule(5, 5);
        action2.Should().Throw<ArgumentException>()
            .WithMessage("Min must be less than Max");
    }

    [Fact]
    public void ParameterizedConstructor_ShouldThrowArgumentException_WhenMinIsNegative()
    {
        // Act
        var action = () => new LengthRule(-1, 5);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("Min cannot be negative");
    }

    [Fact]
    public void ParameterizedConstructor_ShouldThrowArgumentException_WhenMaxIsNegative()
    {
        // Act
        var action = () => new LengthRule(1, -5);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("Max cannot be negative");
    }

    [Fact]
    public void Properties_ShouldBeSettable_WhenValid()
    {
        // Arrange
        var lengthRule = new LengthRule();
        var expectedMin = 3;
        var expectedMax = 50;

        // Act
        lengthRule.Max = expectedMax; // Set Max first
        lengthRule.Min = expectedMin; // Then Min

        // Assert
        lengthRule.Min.Should().Be(expectedMin);
        lengthRule.Max.Should().Be(expectedMax);
    }

    [Fact]
    public void SettingMin_ShouldThrowArgumentException_WhenMinGreaterThanOrEqualToMax()
    {
        // Arrange
        var lengthRule = new LengthRule();
        lengthRule.Max = 10;

        // Act & Assert - Min greater than Max
        var action1 = () => lengthRule.Min = 15;
        action1.Should().Throw<ArgumentException>()
            .WithMessage("*Min must be less than Max*")
            .And.ParamName.Should().Be("Min");

        // Act & Assert - Min equal to Max
        var action2 = () => lengthRule.Min = 10;
        action2.Should().Throw<ArgumentException>()
            .WithMessage("*Min must be less than Max*")
            .And.ParamName.Should().Be("Min");
    }

    [Fact]
    public void SettingMin_ShouldThrowArgumentException_WhenMinIsNegative()
    {
        // Arrange
        var lengthRule = new LengthRule();
        lengthRule.Max = 10;

        // Act
        var action = () => lengthRule.Min = -5;

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("*Min cannot be negative*")
            .And.ParamName.Should().Be("Min");
    }

    [Fact]
    public void SettingMax_ShouldThrowArgumentException_WhenMaxLessThanOrEqualToMin()
    {
        // Arrange
        var lengthRule = new LengthRule();
        lengthRule.Max = 10;
        lengthRule.Min = 9;

        // Act & Assert - Max less than Min
        var action1 = () => lengthRule.Max = 5;
        action1.Should().Throw<ArgumentException>()
            .WithMessage("*Max must be greater than Min*")
            .And.ParamName.Should().Be("Max");

        // Act & Assert - Max equal to Min
        var action2 = () => lengthRule.Max = 9;
        action2.Should().Throw<ArgumentException>()
            .WithMessage("*Max must be greater than Min*")
            .And.ParamName.Should().Be("Max");
    }

    [Theory]
    [InlineData(0, 10)]
    [InlineData(1, 100)]
    [InlineData(10, 200)]
    public void Properties_ShouldAcceptVariousValidValues(int min, int max)
    {
        // Act
        var lengthRule = new LengthRule { Max = max, Min = min }; // Set Max first

        // Assert
        lengthRule.Min.Should().Be(min);
        lengthRule.Max.Should().Be(max);
    }

    [Fact]
    public void CanCreateLengthRuleWithObjectInitializer_WhenMaxSetFirst()
    {
        // Act
        var lengthRule = new LengthRule
        {
            Max = 100,  // Set Max first
            Min = 3     // Then Min
        };

        // Assert
        lengthRule.Min.Should().Be(3);
        lengthRule.Max.Should().Be(100);
    }

    [Fact]
    public void ObjectInitializer_ShouldThrowException_WhenMinSetBeforeMaxWithInvalidValues()
    {
        // Act
        var action = () => new LengthRule
        {
            Min = 10,   // This will fail because Max is still 0
        };

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("*Min must be less than Max*");
    }

    [Fact]
    public void ObjectInitializer_ShouldThrowException_WhenTryingToSetEqualValues()
    {
        // Act
        var action = () => new LengthRule
        {
            Max = 5,
            Min = 5   // This will fail because Max equals Min
        };

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("*Min must be less than Max*");
    }

    [Fact]
    public void ObjectInitializer_ShouldThrowException_WhenSettingNegativeValues()
    {
        // Act & Assert - Negative Min
        var action1 = () => new LengthRule
        {
            Min = -5,
            Max = 10
        };
        action1.Should().Throw<ArgumentException>()
            .WithMessage("*Min cannot be negative*");

        // Act & Assert - Negative Max
        var action2 = () => new LengthRule
        {
            Max = -10,
            Min = 5
        };
        action2.Should().Throw<ArgumentException>()
            .WithMessage("*Max cannot be negative*");
    }

    [Fact]
    public void MultipleInstances_ShouldHaveIndependentValues()
    {
        // Act
        var rule1 = new LengthRule { Max = 10, Min = 1 };
        var rule2 = new LengthRule { Max = 50, Min = 5 };

        // Assert
        rule1.Min.Should().Be(1);
        rule1.Max.Should().Be(10);
        rule2.Min.Should().Be(5);
        rule2.Max.Should().Be(50);
    }

    [Fact]
    public void SettingMinFirst_ThenMax_ShouldWork_WhenValuesAreValid()
    {
        // Arrange
        var lengthRule = new LengthRule();

        // Act
        lengthRule.Min = 0;  // Set Min first to 0 (Max is 0, validation allows this)
        lengthRule.Max = 10; // Then set Max to a valid value (> Min)

        // Assert
        lengthRule.Min.Should().Be(0);
        lengthRule.Max.Should().Be(10);
    }

    [Fact]
    public void SettingMaxFirst_ThenMin_ShouldWork_WhenValuesAreValid()
    {
        // Arrange
        var lengthRule = new LengthRule();

        // Act
        lengthRule.Max = 10; // Set Max first
        lengthRule.Min = 5;  // Then set Min to a valid value (< Max)

        // Assert
        lengthRule.Min.Should().Be(5);
        lengthRule.Max.Should().Be(10);
    }
}