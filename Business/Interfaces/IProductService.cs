using Business.Dtos;

namespace Business.Interfaces;

public interface IProductService
{
    /// <summary>
    /// Creates the new product. If store does not exist, it created it to.
    /// </summary>
    /// <param name="product">The product data object that contains the details of the product to be created</param>
    /// <returns>Returns true if the product is created, otherwise false</returns>
    bool CreateProduct(ProductDto product);

    /// <summary>
    /// Gets all products
    /// </summary>
    /// <returns>Returns all products</returns>
    IEnumerable<ProductDto> GetAll();

    /// <summary>
    /// Gets one contact by the Id provided
    /// </summary>
    /// <param name="product">The product being fetched</param>
    /// <returns>Returns ProductDto otherwise null</returns>
    ProductDto GetOne(ProductDto product);

     /// <summary>
     /// Updates the contact 
     /// </summary>
     /// <param name="product">The product being updated</param>
     /// <returns>Returns the product, otherwise null</returns>
    ProductDto Update(ProductDto product);

    /// <summary>
    /// Removes a product by the productName provided.
    /// </summary>
    /// <param name="productName">The productName of the product being removed</param>
    /// <returns>Returns true if product is removed, otherwise false</returns>
    bool Remove(string productName);
}
