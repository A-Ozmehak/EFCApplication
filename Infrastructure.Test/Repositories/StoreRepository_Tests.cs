using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Test.Repositories;

public class StoreRepository_Tests
{
    private readonly ProductCatalogContext _context =
       new ProductCatalogContext(new DbContextOptionsBuilder<ProductCatalogContext>()
          .UseInMemoryDatabase($"{Guid.NewGuid()}")
           .Options);

    [Fact]
    public void Create_ShouldCreateAndSaveProductToDatabase_ReturnProduct()
    {
        // Arrange
        IStoreRepository storeRepository = new StoreRepository(_context);
        var storeEntity = new StoresEntity { StoreName = "Willys" };

        // Act
        var result = storeRepository.Create(storeEntity);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void Create_ShouldNotSaveProductToDatabase_ReturnNull()
    {
        // Arrange
        IStoreRepository storeRepository = new StoreRepository(_context);
        var storeEntity = new StoresEntity { };

        // Act
        var result = storeRepository.Create(storeEntity);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Get_ShouldGetAllProducts_ReturnIEnumerableOfTypeProductsEntity()
    {
        // Arrange
        IStoreRepository storeRepository = new StoreRepository(_context);
        var storeEntity = new StoresEntity { StoreName = "Willys" };
        storeRepository.Create(storeEntity);

        // Act
        var result = storeRepository.GetAll();

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<StoresEntity>>(result);
    }

    [Fact]
    public void Get_ShouldFindOneProductMyProductName_ReturnOneProduct()
    {
        // Arrange
        IStoreRepository storeRepository = new StoreRepository(_context);
        var storeEntity = new StoresEntity { StoreName = "Willys" };
        storeRepository.Create(storeEntity);

        // Act
        var result = storeRepository.GetOne(x => x.StoreName == storeEntity.StoreName);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(storeEntity.StoreName, result.StoreName);
    }

    [Fact]
    public void Get_ShouldNotFindOneProductByProductName_ReturnNull()
    {
        // Arrange
        IStoreRepository storeRepository = new StoreRepository(_context);
        var storeEntity = new StoresEntity { StoreName = "Willys" };

        // Act
        var result = storeRepository.GetOne(x => x.StoreName == storeEntity.StoreName);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Delete_ShouldRemoveOneProduct_ReturnTrue()
    {
        // Arrange
        IStoreRepository storeRepository = new StoreRepository(_context);
        var storeEntity = new StoresEntity { StoreName = "Willys" };
        storeRepository.Create(storeEntity);

        // Act
        var result = storeRepository.Delete(x => x.StoreName == storeEntity.StoreName);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Delete_ShouldNotFindProductAndRemoveIt_ReturnFalse()
    {
        // Arrange
        IStoreRepository storeRepository = new StoreRepository(_context);
        var storeEntity = new StoresEntity { StoreName = "Willys" };

        // Act
        var result = storeRepository.Delete(x => x.StoreName == storeEntity.StoreName);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Update_ShouldUpdateExistingProductEntity_ReturnUpdatedContactEntity()
    {
        // Arrange
        IStoreRepository storeRepository = new StoreRepository(_context);
        var storeEntity = new StoresEntity { StoreName = "Willys" };
        storeRepository.Create(storeEntity);

        // Act
        storeEntity.StoreName = "Coop";
        var result = storeRepository.Update(storeEntity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(storeEntity.StoreName, result.StoreName);
    }
}
