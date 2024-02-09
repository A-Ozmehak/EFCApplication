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
    public void Create_ShouldCreateProductAndSaveProduct_ReturnTrue()
    {
        // Arrange
        var mockProductRepository = new Mock<IProductRepository>();
        var mockStoreRepository = new Mock<IStoreRepository>();
        IProductService productService = new ProductService(mockProductRepository.Object, mockStoreRepository.Object);

        var productDto = new ProductDto { ProductName = "Milk", Price = 23, StoreName = "Willys" };

        mockStoreRepository.Setup(x => x.Create(It.IsAny<StoresEntity>())).Returns(new StoresEntity());
        mockProductRepository.Setup(x => x.Create(It.IsAny<ProductsEntity>())).Returns(new ProductsEntity());

        // Act
        bool result = productService.CreateProduct(productDto);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Create_ShouldNotCreateAndSaveProduct_ReturnFalse()
    {
        // Arrange
        var mockProductRepository = new Mock<IProductRepository>();
        var mockStoreRepository = new Mock<IStoreRepository>();
        IProductService productService = new ProductService(mockProductRepository.Object, mockStoreRepository.Object);

        var productDto = new ProductDto { ProductName = "Milk", Price = 23, StoreName = "Willys" };

        // Act
        var result = productService.CreateProduct(productDto);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void GetAll_ShouldGetAllProducts_ReturnIEnumerableOfTypeProducts()
    {
        // Arrange
        var mockProductRepository = new Mock<IProductRepository>();
        var mockStoreRepository = new Mock<IStoreRepository>();
        IProductService productService = new ProductService(mockProductRepository.Object, mockStoreRepository.Object);

        var products = new List<ProductsEntity>
        {
            new ProductsEntity { ProductName = "Milk", Price = 23, Store = new StoresEntity { StoreName = "Willys" } },
            new ProductsEntity {  ProductName = "Bread", Price = 15, Store = new StoresEntity { StoreName = "Coop" }},
        };

        mockProductRepository.Setup(x => x.GetAll()).Returns(products);

        // Act
        var result = productService.GetAll();

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Contains(result, r => r.ProductName == "Milk" && r.Price == 23 && r.StoreName == "Willys");
        Assert.Contains(result, r => r.ProductName == "Bread" && r.Price == 15 && r.StoreName == "Coop");
    }

    [Fact]
    public void GetAll_ShouldGetNoProducts_ReturnEmptyIEnumerable()
    {
        // Arrange
        var mockProductRepository = new Mock<IProductRepository>();
        var mockStoreRepository = new Mock<IStoreRepository>();
        IProductService productService = new ProductService(mockProductRepository.Object, mockStoreRepository.Object);

        var products = new List<ProductsEntity>();

        mockProductRepository.Setup(x => x.GetAll()).Returns(products);

        // Act
        var result = productService.GetAll();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void Get_ShouldFindOneProductById_ReturnOneProduct()
    {
        // Arrange
        var mockProductRepository = new Mock<IProductRepository>();
        var mockStoreRepository = new Mock<IStoreRepository>();
        IProductService productService = new ProductService(mockProductRepository.Object, mockStoreRepository.Object);

        var productEntity = new ProductsEntity
        {
            Id = 1,
            ProductName = "Test Product",
            Price = 10.0m,
            Store = new StoresEntity { StoreName = "Test Store" }
        };

        mockProductRepository.Setup(x => x.GetOneById(It.IsAny<Expression<Func<ProductsEntity, bool>>>()))
            .Returns(productEntity);

        // Act
        var returnedProductDto = productService.GetOne(productEntity);

        // Assert
        Assert.NotNull(returnedProductDto);
        Assert.Equal(productEntity.Id, returnedProductDto.Id);
        Assert.Equal(productEntity.ProductName, returnedProductDto.ProductName);
        Assert.Equal(productEntity.Price, returnedProductDto.Price);
        Assert.Equal(productEntity.Store.StoreName, returnedProductDto.StoreName);
    }

    [Fact]
    public void Get_ShouldNotFindOneProductById_ReturnNull()
    {
        // Arrange
        var mockProductRepository = new Mock<IProductRepository>();
        var mockStoreRepository = new Mock<IStoreRepository>();
        IProductService productService = new ProductService(mockProductRepository.Object, mockStoreRepository.Object);

        mockProductRepository.Setup(repo => repo.GetOneById(It.IsAny<Expression<Func<ProductsEntity, bool>>>()))
            .Returns((ProductsEntity)null);

        var productDtoToGet = new ProductDto { Id = 1 };

        // Act
        var returnedProductDto = productService.GetOne(productDtoToGet);

        // Assert
        Assert.Null(returnedProductDto);
    }

    [Fact]
    public void Remove_ShouldRemoveOneProduct_ReturnTrue()
    {
        // Arrange
        var mockProductRepository = new Mock<IProductRepository>();
        var mockStoreRepository = new Mock<IStoreRepository>();
        IProductService productService = new ProductService(mockProductRepository.Object, mockStoreRepository.Object);

        var productEntity = new ProductsEntity { ProductName = "Milk", Price = 23, Store = new StoresEntity { StoreName = "Willys"} };

        mockProductRepository.Setup(x => x.GetOne(It.IsAny<Expression<Func<ProductsEntity, bool>>>())).Returns(productEntity);
        mockProductRepository.Setup(x => x.Delete(It.Is<Expression<Func<ProductsEntity, bool>>>(predicate => predicate.Compile()(productEntity)))).Returns(true);

        // Act
        var result = productService.Remove(productEntity.ProductName);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Remove_ShouldNotFindProductAndRemoveIt_ReturnFalse()
    {
        // Arrange
        var mockProductRepository = new Mock<IProductRepository>();
        var mockStoreRepository = new Mock<IStoreRepository>();
        IProductService productService = new ProductService(mockProductRepository.Object, mockStoreRepository.Object);

        var nonExistingProductName = "Ketchup";

        mockProductRepository.Setup(x => x.GetOne(It.IsAny<Expression<Func<ProductsEntity, bool>>>())).Returns((ProductsEntity)null);


        // Act
        productService.Remove(nonExistingProductName);

        // Assert
        mockProductRepository.Verify(x => x.Delete(It.IsAny<Expression<Func<ProductsEntity, bool>>>()), Times.Never);
    }

    [Fact]
    public void Update_ShouldUpdateExistingProductEntity_ReturnUpdatedProductEntity()
    {
        // Arrange
        var productDto = new ProductDto
        {
            Id = 1,
            ProductName = "Milk",
            Price = 10,
            StoreName = "Coop"
        };

        var existingProductEntity = new ProductsEntity
        {
            Id = productDto.Id,
            ProductName = "Bread",
            Price = 15,
            Store = new StoresEntity { StoreName = "Willys" }
        };

        var updatedProductEntity = new ProductsEntity
        {
            Id = productDto.Id,
            ProductName = productDto.ProductName,
            Price = productDto.Price,
            Store = new StoresEntity { StoreName = productDto.StoreName }
        };

        var mockProductRepository = new Mock<IProductRepository>();
        mockProductRepository.Setup(repo => repo.GetOne(It.IsAny<Expression<Func<ProductsEntity, bool>>>()))
            .Returns(existingProductEntity);
        mockProductRepository.Setup(repo => repo.Update(It.IsAny<ProductsEntity>()))
            .Returns(updatedProductEntity);

        var mockStoreRepository = new Mock<IStoreRepository>();
        mockStoreRepository.Setup(repo => repo.GetOne(It.IsAny<Expression<Func<StoresEntity, bool>>>()))
            .Returns((StoresEntity)null);
        mockStoreRepository.Setup(repo => repo.Create(It.IsAny<StoresEntity>()))
            .Returns(updatedProductEntity.Store);

        var productService = new ProductService(mockProductRepository.Object, mockStoreRepository.Object);

        // Act
        var returnedProductDto = productService.Update(productDto);

        // Assert
        Assert.NotNull(returnedProductDto);
        Assert.Equal(updatedProductEntity.Id, returnedProductDto.Id);
        Assert.Equal(updatedProductEntity.ProductName, returnedProductDto.ProductName);
        Assert.Equal(updatedProductEntity.Price, returnedProductDto.Price);
    }

    [Fact]
    public void Update_ShouldNotUpdateProduct_WhenProductDoesNotExist()
    {
        // Arrange
        var mockProductRepository = new Mock<IProductRepository>();
        var mockStoreRepository = new Mock<IStoreRepository>();
        IProductService productService = new ProductService(mockProductRepository.Object, mockStoreRepository.Object);

        var nonExistentProductDto = new ProductDto { ProductName = "Non", Price = 1, StoreName = "Existent" };

        mockProductRepository.Setup(x => x.GetOne(It.IsAny<Expression<Func<ProductsEntity, bool>>>())).Returns((ProductsEntity)null);

        // Act
        var result = productService.Update(nonExistentProductDto);

        // Assert
        Assert.Null(result);
        mockProductRepository.Verify(x => x.Update(It.IsAny<ProductsEntity>()), Times.Never);
    }

}
