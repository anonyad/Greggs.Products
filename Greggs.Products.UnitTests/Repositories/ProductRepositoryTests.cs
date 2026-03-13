using System.Collections.Generic;
using System.Linq;
using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Models;
using Greggs.Products.Api.Repositories;
using Moq;
using Xunit;

namespace Greggs.Products.UnitTests.Repositories;

public class ProductRepositoryTests
{
    private readonly Mock<IDataAccess<Product>> _mockDataAccess;
    private readonly ProductRepository _productRepository;

    public ProductRepositoryTests()
    {
        _mockDataAccess = new Mock<IDataAccess<Product>>();
        _productRepository = new ProductRepository(_mockDataAccess.Object);
    }

    [Fact]
    public void GetProducts_CallsDataAccessWithCorrectParameters()
    {
        // Arrange
        var products = new List<Product>();
        _mockDataAccess.Setup(d => d.List(0, 5)).Returns(products);

        // Act
        var result = _productRepository.GetProducts(0, 5);

        // Assert
        _mockDataAccess.Verify(d => d.List(0, 5), Times.Once);
    }

    [Fact]
    public void GetProducts_ReturnsDataAccessResult()
    {
        // Arrange
        var expectedProducts = new List<Product>
        {
            new() { Name = "Sausage Roll", PriceInPounds = 1.00m },
            new() { Name = "Steak Bake", PriceInPounds = 1.20m }
        };
        _mockDataAccess.Setup(d => d.List(0, 5)).Returns(expectedProducts);

        // Act
        var result = _productRepository.GetProducts(0, 5);

        // Assert
        Assert.Equal(expectedProducts, result);
    }

    [Fact]
    public void GetProducts_WithPaginationParameters_PassesThroughCorrectly()
    {
        // Arrange
        var products = new List<Product>();
        _mockDataAccess.Setup(d => d.List(10, 20)).Returns(products);

        // Act
        _productRepository.GetProducts(10, 20);

        // Assert
        _mockDataAccess.Verify(d => d.List(10, 20), Times.Once);
    }
}
