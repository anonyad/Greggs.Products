using System;
using System.Collections.Generic;
using System.Linq;
using Greggs.Products.Api.Enums;
using Greggs.Products.Api.Models;
using Greggs.Products.Api.Repositories;

namespace Greggs.Products.Api.Services;

public class ProductService : IProductService
{
    private const decimal GbpToEurRate = 1.11m;
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public IEnumerable<Product> GetProductsInEuros(int pageStart, int pageSize, Currency currency)
    {
        var products = _productRepository.GetProducts(pageStart, pageSize);

        if (currency == Currency.EUR)
        {
            return products.Select(p => new Product
            {
                Name = p.Name,
                PriceInPounds = Math.Round(p.PriceInPounds * GbpToEurRate, 2)
            });
        }

        return products;
    }
}
