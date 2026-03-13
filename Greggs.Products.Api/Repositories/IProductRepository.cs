using System.Collections.Generic;
using Greggs.Products.Api.Models;

namespace Greggs.Products.Api.Repositories;

public interface IProductRepository
{
    IEnumerable<Product> GetProducts(int pageStart, int pageSize);
}
