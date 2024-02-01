using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Business.Test.Services;

public class ProductService_Tests
{
    private readonly ProductCatalogContext _context =
    new ProductCatalogContext(new DbContextOptionsBuilder<ProductCatalogContext>()
       .UseInMemoryDatabase($"{Guid.NewGuid()}")
        .Options);

    [Fact]
    public void Create_ShouldCreateProductToRepository_ReturnProduct()
    {
        // Arrange
        IProductRepository productRepository = new ProductRepository(_context);
        var storeEntity = new StoresEntity { StoreName = "Coop" };
        var productEntity = new ProductsEntity { ProductName = "Milk", Price = 23, Store = storeEntity };

        // Act
        var result = productRepository.Create(productEntity);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void Create_ShouldNotCreateProductToRepository_ReturnNull()
    {
        // Arrange
        IProductRepository productRepository = new ProductRepository(_context);
        var storeEntity = new StoresEntity();
        var productEntity = new ProductsEntity();

        // Act
        var result = productRepository.Create(productEntity);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Get_ShouldGetAllProducts_ReturnIEnumerableOfTypeProducts()
    {
        // Arrange
        IProductRepository productRepository = new ProductRepository(_context);
        var storeEntity = new StoresEntity { StoreName = "Coop" };   
        var productEntity = new ProductsEntity { ProductName = "Milk", Price = 23, Store = storeEntity };
        productRepository.Create(productEntity);

        // Act
        var result = productRepository.GetAll();

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<ProductsEntity>>(result);
    }

    [Fact]
    public void Get_ShouldFindOneProductMyProductName_ReturnOneProduct()
    {
        // Arrange
        IProductRepository productRepository = new ProductRepository(_context);
        var storeEntity = new StoresEntity { StoreName = "Coop" };
        var productEntity = new ProductsEntity { ProductName = "Milk", Price = 23, Store = storeEntity };
        productRepository.Create(productEntity);

        // Act
        var result = productRepository.GetOne(x => x.ProductName == productEntity.ProductName);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(productEntity.ProductName, result.ProductName);
    }

    [Fact]
    public void Get_ShouldNotFindOneProductByProductName_ReturnNull()
    {
        // Arrange
        IProductRepository productRepository = new ProductRepository(_context);
        var storeEntity = new StoresEntity { StoreName = "Coop" };
        var productEntity = new ProductsEntity { ProductName = "Milk", Price = 23, Store = storeEntity };

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
        var storeEntity = new StoresEntity { StoreName = "Coop" };
        var productEntity = new ProductsEntity { ProductName = "Milk", Price = 23, Store = storeEntity };
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
        var storeEntity = new StoresEntity { StoreName = "Coop" };
        var productEntity = new ProductsEntity { ProductName = "Milk", Price = 23, Store = storeEntity };

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
        var storeEntity = new StoresEntity { StoreName = "Coop" };
        var productEntity = new ProductsEntity { ProductName = "Milk", Price = 23, Store = storeEntity };
        productRepository.Create(productEntity);

        // Act
        productEntity.ProductName = "Bread";
        var result = productRepository.Update(productEntity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(productEntity.ProductName, result.ProductName);
    }
}
