using System.Collections.Generic;
using System.Linq;
using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Models;

namespace Greggs.Products.Api.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly IDataAccess<Product> _dataAccess;

    public ProductRepository(IDataAccess<Product> dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public IEnumerable<Product> GetProducts(int pageStart, int pageSize)
    {
        return _dataAccess.List(pageStart, pageSize);
    }
}
