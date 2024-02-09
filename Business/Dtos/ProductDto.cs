using Infrastructure.Entities;
using System.Security.Cryptography.X509Certificates;

namespace Business.Dtos;

public class ProductDto
{
    public int Id { get; set; }
    public string ProductName { get; set; } = null!;
    public decimal? Price { get; set; }
    public string StoreName { get; set; } = null!;


    public static implicit operator ProductDto(ProductsEntity entity)
    {
        var productDto = new ProductDto
        {
            Id = entity.Id,
            ProductName = entity.ProductName,
            Price = entity.Price,
            StoreName = entity.Store.StoreName,
        };
        return productDto;
    }
}
