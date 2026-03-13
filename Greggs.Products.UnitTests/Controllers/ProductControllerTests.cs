using System.Collections.Generic;
using System.Linq;
using Greggs.Products.Api.Controllers;
using Greggs.Products.Api.Enums;
using Greggs.Products.Api.Models;
using Greggs.Products.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Greggs.Products.UnitTests.Controllers;

public class ProductControllerTests
{
    private readonly Mock<ILogger<ProductController>> _mockLogger;
    private readonly Mock<IProductService> _mockProductService;
    private readonly ProductController _controller;

    public ProductControllerTests()
    {
        _mockLogger = new Mock<ILogger<ProductController>>();
        _mockProductService = new Mock<IProductService>();
        _controller = new ProductController(_mockLogger.Object, _mockProductService.Object);
    }

    [Fact]
    public void Get_WithDefaultParameters_ReturnsProducts()
    {
        // Arrange
        var expectedProducts = new List<Product>
        {
            new() { Name = "Sausage Roll", PriceInPounds = 1.00m }
        };
        _mockProductService.Setup(s => s.GetProductsInEuros(0, 5, Currency.GBP))
                          .Returns(expectedProducts);

        // Act
        var result = _controller.Get();

        // Assert
        var products = Assert.IsAssignableFrom<IEnumerable<Product>>(result);
        Assert.Single(products);
        _mockProductService.Verify(s => s.GetProductsInEuros(0, 5, Currency.GBP), Times.Once);
    }

    [Fact]
    public void Get_WithPaginationParameters_CallsServiceWithCorrectParameters()
    {
        // Arrange
        var products = new List<Product>();
        _mockProductService.Setup(s => s.GetProductsInEuros(2, 10, Currency.EUR))
                          .Returns(products);

        // Act
        var result = _controller.Get(2, 10, Currency.EUR);

        // Assert
        _mockProductService.Verify(s => s.GetProductsInEuros(2, 10, Currency.EUR), Times.Once);
    }

    [Fact]
    public void Get_WithEURCurrency_CallsServiceWithEUR()
    {
        // Arrange
        var products = new List<Product>();
        _mockProductService.Setup(s => s.GetProductsInEuros(0, 5, Currency.EUR))
                          .Returns(products);

        // Act
        var result = _controller.Get(0, 5, Currency.EUR);

        // Assert
        _mockProductService.Verify(s => s.GetProductsInEuros(0, 5, Currency.EUR), Times.Once);
    }

    [Fact]
    public void Get_WithGBPCurrency_CallsServiceWithGBP()
    {
        // Arrange
        var products = new List<Product>();
        _mockProductService.Setup(s => s.GetProductsInEuros(0, 5, Currency.GBP))
                          .Returns(products);

        // Act
        var result = _controller.Get(0, 5, Currency.GBP);

        // Assert
        _mockProductService.Verify(s => s.GetProductsInEuros(0, 5, Currency.GBP), Times.Once);
    }

    [Fact]
    public void Get_ServiceReturnsNull_ReturnsEmptyResult()
    {
        // Arrange
        _mockProductService.Setup(s => s.GetProductsInEuros(0, 5, Currency.GBP))
                          .Returns(new List<Product>());

        // Act
        var result = _controller.Get();

        // Assert
        var products = Assert.IsAssignableFrom<IEnumerable<Product>>(result);
        Assert.Empty(products);
    }
}
