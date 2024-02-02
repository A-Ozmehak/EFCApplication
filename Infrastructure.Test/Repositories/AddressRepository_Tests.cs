using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Test.Repositories;

public class AddressRepository_Tests
{
    private readonly ContactContext _context =
        new ContactContext(new DbContextOptionsBuilder<ContactContext>()
            .UseInMemoryDatabase($"{Guid.NewGuid()}")
            .Options);

    [Fact]
    public void Create_ShouldCreateAndSaveAddressToDatabase_ReturnAddress()
    {
        // Arrange
        IAddressRepository addressRepository = new AddressRepository(_context);
        var addressEntity = new AddressEntity { StreetName = "Gustaf dalensgatan", StreetNumber = "24", PostalCode = "41724", City = "Goteborg" };

        // Act
        var result = addressRepository.Create(addressEntity);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void Get_ShouldGetAllAddresses_ReturnIEnumerableOfTypeAddressEntity()
    {
        // Arrange
        IAddressRepository addressRepository = new AddressRepository(_context);
        var addressEntity = new AddressEntity { StreetName = "Gustaf dalensgatan", StreetNumber = "24", PostalCode = "41724", City = "Goteborg" };
        addressRepository.Create(addressEntity);

        // Act
        var result = addressRepository.GetAll();

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<AddressEntity>>(result);
    }

    [Fact]
    public void Get_ShouldFindOneAddressByStreetName_ReturnOneAddress()
    {
        // Arrange
        IAddressRepository addressRepository = new AddressRepository(_context);
        var addressEntity = new AddressEntity { StreetName = "Gustaf dalensgatan", StreetNumber = "24", PostalCode = "41724", City = "Goteborg" };
        addressRepository.Create(addressEntity);

        // Act
        var result = addressRepository.GetOne(x => x.StreetName == addressEntity.StreetName);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(addressEntity.StreetName, result.StreetName);
    }

    [Fact]
    public void Get_ShouldNotFindOneAddressByStreetName_ReturnNull()
    {
        // Arrange
        IAddressRepository addressRepository = new AddressRepository(_context);
        var addressEntity = new AddressEntity { StreetName = "Gustaf dalensgatan", StreetNumber = "24", PostalCode = "41724", City = "Goteborg" };

        // Act
        var result = addressRepository.GetOne(x => x.StreetName == addressEntity.StreetName);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Delete_ShouldRemoveOneAddress_ReturnTrue()
    {
        // Arrange
        IAddressRepository addressRepository = new AddressRepository(_context);
        var addressEntity = new AddressEntity { StreetName = "Gustaf dalensgatan", StreetNumber = "24", PostalCode = "41724", City = "Goteborg" };
        addressRepository.Create(addressEntity);

        // Act
        var result = addressRepository.Delete(x => x.StreetName == addressEntity.StreetName);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Delete_ShouldNotFindAddressAndRemoveIt_ReturnFalse()
    {
        // Arrange
        IAddressRepository addressRepository = new AddressRepository(_context);
        var addressEntity = new AddressEntity { StreetName = "Gustaf dalensgatan", StreetNumber = "24", PostalCode = "41724", City = "Goteborg" };

        // Act
        var result = addressRepository.Delete(x => x.StreetName == addressEntity.StreetName);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Update_ShouldUpdateExistingAddressEntity_ReturnUpdatedAddressEntity()
    {
        // Arrange
        IAddressRepository addressRepository = new AddressRepository(_context);
        var addressEntity = new AddressEntity { StreetName = "Gustaf dalensgatan", StreetNumber = "24", PostalCode = "41724", City = "Goteborg" };
        addressRepository.Create(addressEntity);

        // Act
        addressEntity.StreetName = "Storgatan";
        addressEntity.StreetNumber = "2";
        var result = addressRepository.Update(addressEntity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(addressEntity.StreetName, result.StreetName);
        Assert.Equal(addressEntity.StreetNumber, result.StreetNumber);
    }

    [Fact]
    public void Exists_ShouldCheckIfAddressExists_ReturnTrueIfAddressExists()
    {
        // Arrange
        IAddressRepository addressRepository = new AddressRepository(_context);
        var addressEntity = new AddressEntity { StreetName = "Gustaf dalensgatan", StreetNumber = "24", PostalCode = "41724", City = "Goteborg" };
        addressRepository.Create(addressEntity);

        // Act
        bool exists = addressRepository.Exists(x => x.Id == addressEntity.Id);

        // Assert
        Assert.True(exists);
    }

    [Fact]
    public void Exists_ShouldReturnFalse_WhenAddressDoesNotExist()
    {
        // Arrange
        IAddressRepository addressRepository = new AddressRepository(_context);
        int nonExistentAddressId = 999;

        // Act
        bool exists = addressRepository.Exists(x => x.Id == nonExistentAddressId);

        // Assert
        Assert.False(exists);
    }
}
