using System.Collections.Generic;
using Greggs.Products.Api.Enums;
using Greggs.Products.Api.Models;

namespace Greggs.Products.Api.Services;

public interface IProductService
{
    IEnumerable<Product> GetProductsInEuros(int pageStart, int pageSize, Currency currency);
}
