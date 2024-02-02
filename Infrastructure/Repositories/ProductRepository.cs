using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ProductRepository : Repository<ProductsEntity, ProductCatalogContext>, IProductRepository
{
    private readonly ProductCatalogContext _context;

    public ProductRepository(ProductCatalogContext context) : base(context)
    {
        _context = context;
    }

    public override List<ProductsEntity> GetAll()
    {
        return _context.ProductsEntities
            .Include(product => product.Store)
            .ToList();
    }

    public ProductsEntity GetOneByProductName(string productName)
    {
        return _context.ProductsEntities
            .Include(product => product.Store)
            .SingleOrDefault(product => product.ProductName == productName)!;
    }
}
