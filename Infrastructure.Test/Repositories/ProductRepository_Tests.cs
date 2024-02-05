using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Test.Repositories;

public class ProductRepository_Tests
{
    private readonly ProductCatalogContext _context =
        new ProductCatalogContext(new DbContextOptionsBuilder<ProductCatalogContext>()
           .UseInMemoryDatabase($"{Guid.NewGuid()}")
            .Options);

    [Fact]
    public void Create_ShouldCreateAndSaveProductToDatabase_ReturnProduct()
    {
        // Arrange
        IProductRepository productRepository = new ProductRepository(_context);
        var productEntity = new ProductsEntity { ProductName = "Milk", Price = 23 };

        // Act
        var result = productRepository.Create(productEntity);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void Create_ShouldNotSaveProductToDatabase_ReturnNull()
    {
        // Arrange
        IProductRepository productRepository = new ProductRepository(_context);
        var productEntity = new ProductsEntity { };

        // Act
        var result = productRepository.Create(productEntity);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetAll_ShouldGetAllProducts_ReturnIEnumerableOfTypeProductsEntity()
    {
        // Arrange
        IProductRepository productRepository = new ProductRepository(_context);
        var productEntity = new ProductsEntity { ProductName = "Milk", Price = 23 };
        productRepository.Create(productEntity);

        // Act
        var result = productRepository.GetAll();

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<ProductsEntity>>(result);
    }

    [Fact]
    public void GetAll_WhenNoProductsExist_ShouldReturnEmptyCollection()
    {
        // Arrange
        IProductRepository productRepository = new ProductRepository(_context);

        // Act
        var result = productRepository.GetAll();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void GetOneByProductName_ShouldFindOneProductMyProductName_ReturnOneProduct()
    {
        // Arrange
        IProductRepository productRepository = new ProductRepository(_context);
        var productEntity = new ProductsEntity { ProductName = "Milk", Price = 23 };
        productRepository.Create(productEntity);

        // Act
        var result = productRepository.GetOne(x => x.ProductName == productEntity.ProductName);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(productEntity.ProductName, result.ProductName);
    }

    [Fact]
    public void GetOneByProductName_ShouldNotFindOneProductByProductName_ReturnNull()
    {
        // Arrange
        IProductRepository productRepository = new ProductRepository(_context);
        var productEntity = new ProductsEntity { ProductName = "Milk", Price = 23 };

        // Act
        var result = productRepository.GetOne(x => x.ProductName == productEntity.ProductName);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Delete_ShouldRemoveOneProduct_ReturnTrue()
    {
        // Arrange
        IProductRepository productRepository = new ProductRepository(_context);
        var productEntity = new ProductsEntity { ProductName = "Milk", Price = 23 };
        productRepository.Create(productEntity);

        // Act
        var result = productRepository.Delete(x => x.ProductName == productEntity.ProductName);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Delete_ShouldNotFindProductAndRemoveIt_ReturnFalse()
    {
        // Arrange
        IProductRepository productRepository = new ProductRepository(_context);
        var productEntity = new ProductsEntity { ProductName = "Milk", Price = 23 };

        // Act
        var result = productRepository.Delete(x => x.ProductName == productEntity.ProductName);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Update_ShouldUpdateExistingProductEntity_ReturnUpdatedContactEntity() 
    {
        // Arrange
        IProductRepository productRepository = new ProductRepository(_context);
        var productEntity = new ProductsEntity { ProductName = "Milk", Price = 23 };
        productRepository.Create(productEntity);

        // Act
        productEntity.ProductName = "Bread";
        var result = productRepository.Update(productEntity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(productEntity.ProductName, result.ProductName);
    }

    [Fact]
    public void Exists_ShouldCheckIfProductExists_ReturnTrueIfProductExists()
    {
        // Arrange
        IProductRepository productRepository = new ProductRepository(_context);
        var productEntity = new ProductsEntity { ProductName = "Milk", Price = 23 };
        productRepository.Create(productEntity);

        // Act
        bool exists = productRepository.Exists(x => x.Id == productEntity.Id);

        // Assert
        Assert.True(exists);
    }

    [Fact]
    public void Exists_ShouldReturnFalse_WhenProductDoesNotExist()
    {
        // Arrange
        IProductRepository productRepository = new ProductRepository(_context);
        int nonExistentProductId = 999;

        // Act
        bool exists = productRepository.Exists(x => x.Id == nonExistentProductId);

        // Assert
        Assert.False(exists);
    }
}
