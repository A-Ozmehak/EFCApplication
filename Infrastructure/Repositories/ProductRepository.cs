using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories;

public class ProductRepository : Repository<ProductsEntity, ProductCatalogContext>, IProductRepository
{
    private readonly ProductCatalogContext _context;
    public ProductRepository(ProductCatalogContext context) : base(context)
    {
        _context = context;
    }
}
