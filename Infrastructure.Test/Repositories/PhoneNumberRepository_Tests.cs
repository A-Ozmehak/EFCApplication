using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Test.Repositories;

public class PhoneNumberRepository_Tests
{
    private readonly ContactContext _context =
        new ContactContext(new DbContextOptionsBuilder<ContactContext>()
            .UseInMemoryDatabase($"{Guid.NewGuid()}")
            .Options);

    [Fact]
    public void Create_ShouldCreateAndSavePhoneNumberToDatabase_ReturnPhoneNumber()
    {
        // Arrange
        IPhoneNumberRepository phoneNumberRepository = new PhoneNumberRepository(_context);
        var phoneNumberEntity = new PhoneNumberEntity { PhoneNumber = "0793555635" };

        // Act
        var result = phoneNumberRepository.Create(phoneNumberEntity);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void Create_ShouldNotSavePhoneNumberToTheDatabase_ReturnNull()
    {
        // Arrange
        IPhoneNumberRepository phoneNumberRepository = new PhoneNumberRepository(_context);
        var phoneNumberEntity = new PhoneNumberEntity();

        // Act
        var result = phoneNumberRepository.Create(phoneNumberEntity);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Get_ShouldGetAllPhoneNumbers_ReturnIEnumerableOfTypePhoneNumberEntity()
    {
        // Arrange
        IPhoneNumberRepository phoneNumberRepository = new PhoneNumberRepository(_context);
        var phoneNumberEntity = new PhoneNumberEntity { PhoneNumber = "0793555635" };
        phoneNumberRepository.Create(phoneNumberEntity);

        // Act
        var result = phoneNumberRepository.GetAll();

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<PhoneNumberEntity>>(result);
    }

    [Fact]
    public void Get_ShouldFindOnePhoneNumberByPhoneNumber_ReturnOnePhoneNumber()
    {
        // Arrange
        IPhoneNumberRepository phoneNumberRepository = new PhoneNumberRepository(_context);
        var phoneNumberEntity = new PhoneNumberEntity { PhoneNumber = "0793555635" };
        phoneNumberRepository.Create(phoneNumberEntity);

        // Act
        var result = phoneNumberRepository.GetOne(x => x.PhoneNumber == phoneNumberEntity.PhoneNumber);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(phoneNumberEntity.PhoneNumber, result.PhoneNumber);
    }

    [Fact]
    public void Get_ShouldNotFindOnePhoneNumberByPhoneNumber_ReturnNull()
    {
        // Arrange
        IPhoneNumberRepository phoneNumberRepository = new PhoneNumberRepository(_context);
        var phoneNumberEntity = new PhoneNumberEntity { PhoneNumber = "0793555635" };

        // Act
        var result = phoneNumberRepository.GetOne(x => x.PhoneNumber == phoneNumberEntity.PhoneNumber);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Delete_ShouldRemoveOnePhoneNumber_ReturnTrue()
    {
        // Arrange
        IPhoneNumberRepository phoneNumberRepository = new PhoneNumberRepository(_context);
        var phoneNumberEntity = new PhoneNumberEntity { PhoneNumber = "0793555635" };
        phoneNumberRepository.Create(phoneNumberEntity);

        // Act
        var result = phoneNumberRepository.Delete(x => x.PhoneNumber == phoneNumberEntity.PhoneNumber);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Delete_ShouldNotFindPhoneNumberAndRemoveIt_ReturnFalse()
    {
        // Arrange
        IPhoneNumberRepository phoneNumberRepository = new PhoneNumberRepository(_context);
        var phoneNumberEntity = new PhoneNumberEntity { PhoneNumber = "0793555635" };

        // Act
        var result = phoneNumberRepository.Delete(x => x.PhoneNumber == phoneNumberEntity.PhoneNumber);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Update_ShouldUpdateExistingPhoneNumberEntity_ReturnUpdatedPhoneNumberEntity()
    {
        // Arrange
        IPhoneNumberRepository phoneNumberRepository = new PhoneNumberRepository(_context);
        var phoneNumberEntity = new PhoneNumberEntity { PhoneNumber = "0793555635" };
        phoneNumberRepository.Create(phoneNumberEntity);

        // Act
        phoneNumberEntity.PhoneNumber = "Calle";
        var result = phoneNumberRepository.Update(phoneNumberEntity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(phoneNumberEntity.PhoneNumber, result.PhoneNumber);
    }

    [Fact]
    public void Exists_ShouldCheckIfPhoneNumberExists_ReturnTrueIfPhoneNumberExists()
    {
        // Arrange
        IPhoneNumberRepository phoneNumberRepository = new PhoneNumberRepository(_context);
        var phoneNumberEntity = new PhoneNumberEntity { PhoneNumber = "0793555635" };
        phoneNumberRepository.Create(phoneNumberEntity);

        // Act
        bool exists = phoneNumberRepository.Exists(x => x.Id == phoneNumberEntity.Id);

        // Assert
        Assert.True(exists);
    }

    [Fact]
    public void Exists_ShouldReturnFalse_WhenPhoneNumberDoesNotExist()
    {
        // Arrange
        IPhoneNumberRepository phoneNumberRepository = new PhoneNumberRepository(_context);
        int nonExistentPhoneNumberId = 999;

        // Act
        bool exists = phoneNumberRepository.Exists(x => x.Id == nonExistentPhoneNumberId);

        // Assert
        Assert.False(exists);
    }
}
