using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

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

    public ProductsEntity GetOneById(Expression<Func<ProductsEntity, bool>> predicate)
    {
        try
        {
            var result = _context.Set<ProductsEntity>()
                .Include(product => product.Store)
                .FirstOrDefault(predicate);

            return result!;
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;    
    }
}
