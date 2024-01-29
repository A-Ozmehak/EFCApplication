using Infrastructure.Entities;
using System.Security.Cryptography.X509Certificates;

namespace Business.Dtos;

public class ProductDto
{
    public string ProductName { get; set; } = null!;
    public decimal? Price { get; set; }
    public string StoreName { get; set; } = null!;
}
