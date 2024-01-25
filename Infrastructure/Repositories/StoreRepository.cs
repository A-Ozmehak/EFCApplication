using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories;

public class StoreRepository : Repository<StoresEntity, ProductCatalogContext>, IStoreRepository
{
    private readonly ProductCatalogContext _context;
    public StoreRepository(ProductCatalogContext context) : base(context)
    {
        _context = context;
    }
}
