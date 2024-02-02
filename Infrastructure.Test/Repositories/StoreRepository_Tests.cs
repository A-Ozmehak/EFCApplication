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
    public void Create_ShouldCreateAndSaveStoreToDatabase_ReturnStore()
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
    public void Create_ShouldNotSaveStoreToDatabase_ReturnNull()
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
    public void Get_ShouldGetAllStores_ReturnIEnumerableOfTypeStoresEntity()
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
    public void Get_ShouldFindOneStoreByStoreName_ReturnOneStore()
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
    public void Get_ShouldNotFindOneStoreByStoreName_ReturnNull()
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
    public void Delete_ShouldRemoveOneStore_ReturnTrue()
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
    public void Delete_ShouldNotFindStoreAndRemoveIt_ReturnFalse()
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
    public void Update_ShouldUpdateExistingStoresEntity_ReturnUpdatedStoresEntity()
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

    [Fact]
    public void Exists_ShouldCheckIfStoreExists_ReturnTrueIfStoreExists()
    {
        // Arrange
        IStoreRepository storeRepository = new StoreRepository(_context);
        var storeEntity = new StoresEntity { StoreName = "Willys" };
        storeRepository.Create(storeEntity);

        // Act
        bool exists = storeRepository.Exists(x => x.Id == storeEntity.Id);

        // Assert
        Assert.True(exists);
    }

    [Fact]
    public void Exists_ShouldReturnFalse_WhenStoreDoesNotExist()
    {
        // Arrange
        IStoreRepository storeRepository = new StoreRepository(_context);
        int nonExistentStoreId = 999;

        // Act
        bool exists = storeRepository.Exists(x => x.Id == nonExistentStoreId);

        // Assert
        Assert.False(exists);
    }
}
