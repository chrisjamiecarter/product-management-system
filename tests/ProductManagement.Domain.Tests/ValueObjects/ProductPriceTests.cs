using ProductManagement.Domain.Errors;
using ProductManagement.Domain.ValueObjects;

namespace ProductManagement.Domain.Tests.ValueObjects;

/// <summary>
/// Unit tests for the <see cref="ProductPrice"/> value object, verifying creation, equality, 
/// and behavior of atomic values and explicit conversion.
/// </summary>
public class ProductPriceTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(0.01)]
    [InlineData(9.99)]
    [InlineData(99.99999)]
    [InlineData(1000000)]
    public void Create_ShouldReturnSuccess_WhenPriceIsValid(decimal validPrice)
    {
        // Arrange.

        // Act.
        var result = ProductPrice.Create(validPrice);

        // Assert.
        Assert.True(result.IsSuccess);
        Assert.Equal(validPrice, result.Value.Value);
    }

    [Theory]
    [InlineData(-0.01)]
    [InlineData(-1)]
    [InlineData(-999.99)]
    public void Create_ShouldReturnFailure_WhenPriceIsNegative(decimal invalidPrice)
    {
        // Arrange.

        // Act.
        var result = ProductPrice.Create(invalidPrice);

        // Assert.
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.ProductPrice.NegativeValue, result.Error);
    }

    [Fact]
    public void ExplicitOperatorDecimal_ShouldReturnValue()
    {
        // Arrange.
        var productPrice = 99.99m;
        var valueObject = ProductPrice.Create(productPrice);

        // Act.
        decimal result = (decimal)valueObject.Value;

        // Assert.
        Assert.Equal(productPrice, result);
    }

    [Fact]
    public void GetAtomicValues_ShouldReturnValue()
    {
        // Arrange.
        var productPrice = 99.99m;
        var valueObject = ProductPrice.Create(productPrice);

        // Act.
        var result = valueObject.Value.GetAtomicValues().ToList();

        // Assert.
        Assert.Single(result);
        Assert.Equal(productPrice, result.First());
    }
    
    [Fact]
    public void Equals_ShouldReturnTrue_WhenSameValue()
    {
        // Arrange.
        var productPrice = 99.99m;

        // Act
        var result1 = ProductPrice.Create(productPrice).Value;
        var result2 = ProductPrice.Create(productPrice).Value;

        // Assert.
        Assert.Equal(result1, result2);
        Assert.Equal(result1.GetHashCode(), result2.GetHashCode());
    }

    [Fact]
    public void Equals_ShouldReturnFalse_WhenDifferentValue()
    {
        // Arrange.
        var productPrice1 = 99.99m;
        var productPrice2 = 149.99m;

        // Act
        var result1 = ProductPrice.Create(productPrice1).Value;
        var result2 = ProductPrice.Create(productPrice2).Value;

        // Assert.
        Assert.NotEqual(result1, result2);
        Assert.NotEqual(result1.GetHashCode(), result2.GetHashCode());
    }
}
