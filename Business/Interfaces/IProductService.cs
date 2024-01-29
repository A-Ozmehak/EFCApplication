using Business.Dtos;

namespace Business.Interfaces;

public interface IProductService
{
    bool CreateProduct(ProductDto product);
    IEnumerable<ProductDto> GetAll();
    ProductDto GetOne(string productName);
    void Update(ProductDto product);
    void Remove(string productName);
}
