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
                    Id = product.Id,
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
                  Id = product.Id,
                  ProductName = product.ProductName,
                  Price = product.Price,
                  StoreName = product.Store.StoreName
                });
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return products;
    }

    public ProductDto GetOne(ProductDto product)
    {
        try
        {
            var entity = _productRepository.GetOneById(x => x.Id == product.Id);
            if (product != null)
            {
                return new ProductDto
                {
                    Id = entity.Id,
                    ProductName = entity.ProductName,
                    Price = entity.Price,
                    StoreName = entity.Store.StoreName
                };
            }

        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }

    public ProductDto Update(ProductDto updatedProductDto)
    {
        try
        {
            var productEntity = _productRepository.GetOne(product => product.Id == updatedProductDto.Id);
            if (productEntity == null)
            {
                return null!;
            }

            var storeEntity = _storeRepository.GetOne(x => x.StoreName == updatedProductDto.StoreName);
            if (storeEntity == null) 
            {
                var newStoreEntity = new StoresEntity { StoreName = updatedProductDto.StoreName };
                storeEntity = _storeRepository.Create(newStoreEntity);
            }

            productEntity.ProductName = updatedProductDto.ProductName;
            productEntity.Price = updatedProductDto.Price;
            productEntity.StoreId = storeEntity.Id;

            _productRepository.Update(productEntity);
            return productEntity;
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }

    public bool Remove(string productName)
    {
        try
        {
            var product = _productRepository.GetOne(x => x.ProductName == productName);

            if (product != null)
            {
                var storeId = product.StoreId;

                _productRepository.Delete(x => x.ProductName == productName);

                var storeIdUsed = _productRepository.Exists(p => p.StoreId == storeId);
                if (!storeIdUsed)
                    _storeRepository.Delete(x => x.Id == storeId);

                return true;    
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return false;
    }
}
