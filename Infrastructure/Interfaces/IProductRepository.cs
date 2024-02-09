using Infrastructure.Entities;
using System.Linq.Expressions;

namespace Infrastructure.Interfaces;

public interface IProductRepository : IRepository<ProductsEntity>
{
    /// <summary>
    /// Gets a product by id and includes the store
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition</param>
    /// <returns>Returns the product otherwise null</returns>
    ProductsEntity GetOneById(Expression<Func<ProductsEntity, bool>> predicate);
}
