using Business.Dtos;
using Business.Interfaces;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using System.Diagnostics;

namespace Business.Services;

public class ProductService(IProductRepository productRepository, IStoreRepository storeRepository) : IProductService
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IStoreRepository _storeRepository = storeRepository;

    public bool CreateProduct(ProductDto product)
    {
        try
        {
            if (!_productRepository.Exists(x => x.ProductName == product.ProductName))
            {
                var storeEntity = _storeRepository.GetOne(x => x.StoreName == product.StoreName);
                storeEntity ??= _storeRepository.Create(new StoresEntity { StoreName = product.StoreName });

                var productEntity = new ProductsEntity
                {
                    ProductName = product.ProductName,
                    Price = product.Price,
                    StoreId = storeEntity.Id
                };

                var result = _productRepository.Create(productEntity);
                if (result != null)
                {
                    return true;
                }
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return false;
    }

    public IEnumerable<ProductDto> GetAll()
    {
        var products = new List<ProductDto>();

        try
        {
            var result = _productRepository.GetAll();

            foreach (var product in result)
            {
                products.Add(new ProductDto
                {
                  ProductName = product.ProductName,
                  Price = product.Price,
                  StoreName = product.Store.StoreName
                });
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return products;
    }

    public ProductDto GetOne(string productName)
    {
        try
        {
            var product = _productRepository.GetOne(x => x.ProductName == productName);
            if (product != null)
            {
                return new ProductDto
                {
                    ProductName = product.ProductName,
                    Price = product.Price,
                    StoreName = product.Store.StoreName
                };
            }

        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }

    public void Update(ProductDto updatedProductDto)
    {
        try
        {
            var existingProduct = _productRepository.GetOne(contact => contact.ProductName == updatedProductDto.ProductName);
            if (existingProduct != null)
            {
                existingProduct.ProductName = updatedProductDto.ProductName;
                existingProduct.Price = updatedProductDto.Price;
                existingProduct.Store.StoreName = updatedProductDto.StoreName;


                _productRepository.Update(existingProduct);
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
    }

    public void Remove(string productName)
    {
        try
        {
            var contact = _productRepository.GetOne(x => x.ProductName == productName);

            if (contact != null)
            {
                _productRepository.Delete(x => x.ProductName == productName);
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }

    }
}
