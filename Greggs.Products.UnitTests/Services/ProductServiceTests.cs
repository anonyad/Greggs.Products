using System.Collections.Generic;
using System.Linq;
using Greggs.Products.Api.Enums;
using Greggs.Products.Api.Models;
using Greggs.Products.Api.Repositories;
using Greggs.Products.Api.Services;
using Moq;
using Xunit;

namespace Greggs.Products.UnitTests.Services;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _mockRepository;
    private readonly ProductService _productService;

    public ProductServiceTests()
    {
        _mockRepository = new Mock<IProductRepository>();
        _productService = new ProductService(_mockRepository.Object);
    }

    [Fact]
    public void GetProductsInEuros_WithGBP_ReturnsOriginalPrices()
    {
        // Arrange
        var products = new List<Product>
        {
            new() { Name = "Sausage Roll", PriceInPounds = 1.00m },
            new() { Name = "Steak Bake", PriceInPounds = 1.20m }
        };
        _mockRepository.Setup(r => r.GetProducts(0, 5)).Returns(products);

        // Act
        var result = _productService.GetProductsInEuros(0, 5, Currency.GBP);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Equal(1.00m, result.First().PriceInPounds);
        Assert.Equal(1.20m, result.Last().PriceInPounds);
    }

    [Fact]
    public void GetProductsInEuros_WithEUR_ConvertsPrices()
    {
        // Arrange
        var products = new List<Product>
        {
            new() { Name = "Sausage Roll", PriceInPounds = 1.00m },
            new() { Name = "Steak Bake", PriceInPounds = 1.20m }
        };
        _mockRepository.Setup(r => r.GetProducts(0, 5)).Returns(products);

        // Act
        var result = _productService.GetProductsInEuros(0, 5, Currency.EUR).ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal(1.11m, result[0].PriceInPounds); // 1.00 * 1.11
        Assert.Equal(1.33m, result[1].PriceInPounds); // 1.20 * 1.11
    }

    [Fact]
    public void GetProductsInEuros_CallsRepositoryWithCorrectParameters()
    {
        // Arrange
        var products = new List<Product>();
        _mockRepository.Setup(r => r.GetProducts(2, 10)).Returns(products);

        // Act
        _productService.GetProductsInEuros(2, 10, Currency.GBP);

        // Assert
        _mockRepository.Verify(r => r.GetProducts(2, 10), Times.Once);
    }

    [Fact]
    public void GetProductsInEuros_EURConversion_RoundsToTwoDecimalPlaces()
    {
        // Arrange
        var products = new List<Product>
        {
            new() { Name = "Test Product", PriceInPounds = 0.99m }
        };
        _mockRepository.Setup(r => r.GetProducts(0, 5)).Returns(products);

        // Act
        var result = _productService.GetProductsInEuros(0, 5, Currency.EUR).First();

        // Assert
        Assert.Equal(1.10m, result.PriceInPounds); // 0.99 * 1.11 = 1.0989, rounded to 1.10
    }
}
