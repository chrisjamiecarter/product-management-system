using ProductManagement.Domain.Errors;
using ProductManagement.Domain.ValueObjects;

namespace ProductManagement.Domain.Tests.ValueObjects;

/// <summary>
/// Unit tests for the <see cref="ProductName"/> value object, verifying creation, equality, 
/// and behavior of atomic values and explicit conversion.
/// </summary>
public class ProductNameTests
{
    [Fact]
    public void Create_ShouldReturnSuccess_WhenNameIsValid()
    {
        // Arrange.
        var validName = "Test Product";

        // Act.
        var result = ProductName.Create(validName);

        // Assert.
        Assert.True(result.IsSuccess);
        Assert.Equal(validName, result.Value.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Create_ShouldReturnFailure_WhenNameIsInvalid(string? invalidName)
    {
        // Arrange.

        // Act.
        var result = ProductName.Create(invalidName!);

        // Assert.
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.ProductName.Empty, result.Error);
    }

    [Fact]
    public void ExplicitOperatorString_ShouldReturnValue()
    {
        // Arrange.
        var productName = "Test Product";
        var valueObject = ProductName.Create(productName);

        // Act.
        string result = (string)valueObject.Value;

        // Assert.
        Assert.Equal(productName, result);
    }

    [Fact]
    public void GetAtomicValues_ShouldReturnValue()
    {
        // Arrange.
        var productName = "Test Product";
        var valueObject = ProductName.Create(productName);

        // Act.
        var result = valueObject.Value.GetAtomicValues().ToList();

        // Assert.
        Assert.Single(result);
        Assert.Equal(productName, result.First());
    }

    [Fact]
    public void Equals_ShouldReturnTrue_WhenSameValue()
    {
        // Arrange.
        var productName = "Test Product";

        // Act
        var result1 = ProductName.Create(productName).Value;
        var result2 = ProductName.Create(productName).Value;

        // Assert.
        Assert.Equal(result1, result2);
        Assert.Equal(result1.GetHashCode(), result2.GetHashCode());
    }

    [Fact]
    public void Equals_ShouldReturnFalse_WhenDifferentValue()
    {
        // Arrange.
        var productName1 = "Test Product 1";
        var productName2 = "Test Product 2";

        // Act
        var result1 = ProductName.Create(productName1).Value;
        var result2 = ProductName.Create(productName2).Value;

        // Assert.
        Assert.NotEqual(result1, result2);
        Assert.NotEqual(result1.GetHashCode(), result2.GetHashCode());
    }
}
