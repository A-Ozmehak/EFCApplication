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
    public void Get_ShouldFindOneProductMyProductName_ReturnOneProduct()
    {
        // Arrange
        var mockProductRepository = new Mock<IProductRepository>();
        var mockStoreRepository = new Mock<IStoreRepository>();
        IProductService productService = new ProductService(mockProductRepository.Object, mockStoreRepository.Object);

        var productEntity = new ProductsEntity { ProductName = "Milk", Price = 23, Store = new StoresEntity { StoreName = "Coop" } };

        mockProductRepository.Setup(x => x.GetOneByProductName(productEntity.ProductName)).Returns(productEntity);

        // Act
        var result = productService.GetOne(productEntity.ProductName);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(productEntity.ProductName, result.ProductName);
        Assert.Equal(productEntity.Price, result.Price);
        Assert.Equal(productEntity.Store.StoreName, result.StoreName);
    }

    [Fact]
    public void Get_ShouldNotFindOneProductByProductName_ReturnNull()
    {
        // Arrange
        var mockProductRepository = new Mock<IProductRepository>();
        var mockStoreRepository = new Mock<IStoreRepository>();
        IProductService productService = new ProductService(mockProductRepository.Object, mockStoreRepository.Object);

        var nonExistentProductName = "Milk";

        mockProductRepository.Setup(x => x.GetOneByProductName(nonExistentProductName)).Returns((ProductsEntity)null);

        // Act
        var result = productService.GetOne(nonExistentProductName);

        // Assert
        Assert.Null(result);
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
        var mockProductRepository = new Mock<IProductRepository>();
        var mockStoreRepository = new Mock<IStoreRepository>();
        IProductService productService = new ProductService(mockProductRepository.Object, mockStoreRepository.Object);

        var productEntity = new ProductsEntity { ProductName = "Milk", Price = 23, Store = new StoresEntity { StoreName = "Willys" } };
        var productDto = new ProductDto { ProductName = "Bread", Price = 15, StoreName = "Coop" };

        mockProductRepository.Setup(x => x.GetOne(It.IsAny<Expression<Func<ProductsEntity, bool>>>())).Returns(productEntity);
        mockProductRepository.Setup(x => x.Update(It.IsAny<ProductsEntity>())).Callback<ProductsEntity>(c => productEntity = c);

        // Act
        var result = productService.Update(productDto);

        // Assert
        Assert.Equal(productDto.ProductName, productEntity.ProductName);
        Assert.Equal(productDto.Price, productEntity.Price);
        Assert.Equal(productDto.StoreName, productEntity.Store.StoreName);
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
        Assert.False(result);
        mockProductRepository.Verify(x => x.Update(It.IsAny<ProductsEntity>()), Times.Never);
    }

}
