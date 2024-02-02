using Infrastructure.Entities;

namespace Infrastructure.Interfaces;

public interface IProductRepository : IRepository<ProductsEntity>
{
    /// <summary>
    /// Gets a product by productName and includes the store
    /// </summary>
    /// <param name="productName">The productName of the product</param>
    /// <returns>Returns the product</returns>
    ProductsEntity GetOneByProductName(string productName);
}
