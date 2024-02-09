using Business.Dtos;
using Business.Interfaces;
using Business.Services;
using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq.Expressions;

namespace Business.Test.Services;

public class ProductService_Tests
{
    private readonly ProductCatalogContext _context =
    new ProductCatalogContext(new DbContextOptionsBuilder<ProductCatalogContext>()
       .UseInMemoryDatabase($"{Guid.NewGuid()}")
        .Options);

    [Fact]
    public void CreateProduct_ShouldCreateProductAndSaveProduct_ReturnTrue()
    {
        // Arrange
        IProductRepository productRepository = new ProductRepository(_context);
        IStoreRepository storeRepository = new StoreRepository(_context);
        IProductService productService = new ProductService(productRepository, storeRepository);

        var productDto = new ProductDto { ProductName = "Milk", Price = 23, StoreName = "Willys" };

        // Act
        var result = productService.CreateProduct(productDto);

        // Assert
        Assert.True(result);
        var createdProduct = _context.ProductsEntities.FirstOrDefault(p => p.ProductName == productDto.ProductName);
        Assert.NotNull(createdProduct);
        Assert.Equal(productDto.Price, createdProduct.Price);
        var relatedStore = _context.StoresEntities.FirstOrDefault(s => s.Id == createdProduct.StoreId);
        Assert.NotNull(relatedStore);
        Assert.Equal(productDto.StoreName, relatedStore.StoreName);
    }

    [Fact]
    public void CreateProduct_ShouldNotCreateIfAlreadyExists_ReturnFalse()
    {
        // Arrange
        var store = new StoresEntity { StoreName = "Willys" };
        _context.StoresEntities.Add(store);
        _context.SaveChanges();

        var existingProduct = new ProductsEntity { ProductName = "Milk", Price = 23, StoreId = store.Id };
        _context.ProductsEntities.Add(existingProduct);
        _context.SaveChanges();

        IProductRepository productRepository = new ProductRepository(_context);
        IStoreRepository storeRepository = new StoreRepository(_context);
        IProductService productService = new ProductService(productRepository, storeRepository);

        var productDto = new ProductDto { ProductName = "Milk", Price = 23, StoreName = "Willys" };

        // Act
        var result = productService.CreateProduct(productDto);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void GetAll_ShouldGetAllProducts_ReturnAllProducts()
    {
        // Arrange
        var store = new StoresEntity { StoreName = "TestStore" };
        _context.StoresEntities.Add(store);
        _context.SaveChangesAsync();

        var firstProduct = new ProductsEntity { ProductName = "Milk", Price = 10, StoreId = store.Id };
        var SecondProduct = new ProductsEntity { ProductName = "TestProduct2", Price = 20, StoreId = store.Id };
        _context.ProductsEntities.AddRange(firstProduct, SecondProduct);
        _context.SaveChangesAsync();

        IProductRepository productRepository = new ProductRepository(_context);
        IStoreRepository storeRepository = new StoreRepository(_context);
        IProductService productService = new ProductService(productRepository, storeRepository);

        // Act
        var result = productService.GetAll().ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, r => r.ProductName == firstProduct.ProductName && r.Price == firstProduct.Price && r.StoreName == store.StoreName);
        Assert.Contains(result, r => r.ProductName == SecondProduct.ProductName && r.Price == SecondProduct.Price && r.StoreName == store.StoreName);
    }

    [Fact]
    public void GetAll_NoProducts_ReturnEmptyList()
    {
        // Arrange
        IProductRepository productRepository = new ProductRepository(_context);
        IStoreRepository storeRepository = new StoreRepository(_context);
        IProductService productService = new ProductService(productRepository, storeRepository);

        // Act
        var result = productService.GetAll().ToList();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void GetOne_ShouldFindOneProductIfItExists_ReturnOneProduct()
    {
        // Arrange
        var store = new StoresEntity { StoreName = "Willys" };
        _context.StoresEntities.Add(store);
        _context.SaveChanges();

        var product = new ProductsEntity { ProductName = "Milk", Price = 23, StoreId = store.Id };
        _context.ProductsEntities.Add(product);
        _context.SaveChanges();

        IProductRepository productRepository = new ProductRepository(_context);
        IStoreRepository storeRepository = new StoreRepository(_context);
        IProductService productService = new ProductService(productRepository, storeRepository);

        var productDto = new ProductDto { Id = product.Id };

        // Act
        var result = productService.GetOne(productDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(product.ProductName, result.ProductName);
        Assert.Equal(product.Price, result.Price);
        Assert.Equal(store.StoreName, result.StoreName);
    }

    [Fact]
    public void GetOne_ShouldNotFindOneProductById_ReturnNull()
    {
        // Arrange
        IProductRepository productRepository = new ProductRepository(_context);
        IStoreRepository storeRepository = new StoreRepository(_context);
        IProductService productService = new ProductService(productRepository, storeRepository);

        var nonExistentProductDto = new ProductDto { Id = 999 };

        // Act
        var result = productService.GetOne(nonExistentProductDto);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Remove_ProductExists_ProductAndStoreAreRemoved()
    {
        // Arrange
        var productName = "TestProduct";
        var store = new StoresEntity { StoreName = "TestStore" };
        _context.StoresEntities.Add(store);
        _context.SaveChanges();

        var product = new ProductsEntity { ProductName = productName, StoreId = store.Id };
        _context.ProductsEntities.Add(product);
        _context.SaveChanges();

        IProductRepository productRepository = new ProductRepository(_context);
        IStoreRepository storeRepository = new StoreRepository(_context);
        IProductService productService = new ProductService(productRepository, storeRepository);

        // Act
        var result = productService.Remove(productName);

        // Assert
        Assert.True(result);
        Assert.False(_context.ProductsEntities.Any(p => p.ProductName == productName));
        Assert.False(_context.StoresEntities.Any(s => s.Id == store.Id));
    }

    [Fact]
    public void Remove_ShouldNotFindProductAndRemoveIt_ReturnFalse()
    {
        // Arrange
        var productName = "NonExistentProduct";

        IProductRepository productRepository = new ProductRepository(_context);
        IStoreRepository storeRepository = new StoreRepository(_context);
        IProductService productService = new ProductService(productRepository, storeRepository);

        // Act
        var result = productService.Remove(productName);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Update_ShouldUpdateExistingProductEntity_ReturnUpdatedProductEntity()
    {
        // Arrange
        var store = new StoresEntity { StoreName = "Willys" };
        _context.StoresEntities.Add(store);
        _context.SaveChanges();

        var product = new ProductsEntity { ProductName = "Milk", Price = 23, StoreId = store.Id };
        _context.ProductsEntities.Add(product);
        _context.SaveChanges();

        IProductRepository productRepository = new ProductRepository(_context);
        IStoreRepository storeRepository = new StoreRepository(_context);
        IProductService productService = new ProductService(productRepository, storeRepository);

        var updatedProductDto = new ProductDto { Id = product.Id, ProductName = "Bread", Price = 20, StoreName = "Coop" };

        // Act
        var result = productService.Update(updatedProductDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(updatedProductDto.ProductName, result.ProductName);
        Assert.Equal(updatedProductDto.Price, result.Price);
        Assert.Equal(updatedProductDto.StoreName, result.StoreName);
    }

    [Fact]
    public void Update_ShouldNotUpdateProduct_WhenProductDoesNotExist()
    {
        // Arrange
        IProductRepository productRepository = new ProductRepository(_context);
        IStoreRepository storeRepository = new StoreRepository(_context);
        IProductService productService = new ProductService(productRepository, storeRepository);

        var nonExistentProductDto = new ProductDto { Id = 999, ProductName = "NonExistentProduct", Price = 20, StoreName = "NonExistentStore" };

        // Act
        var result = productService.Update(nonExistentProductDto);

        // Assert
        Assert.Null(result);
    }

}
