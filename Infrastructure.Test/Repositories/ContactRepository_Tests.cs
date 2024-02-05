using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Test.Repositories;

public class ContactRepository_Tests
{
    private readonly ContactContext _context =
        new ContactContext(new DbContextOptionsBuilder<ContactContext>()
            .UseInMemoryDatabase($"{Guid.NewGuid()}")
            .Options);

    [Fact]
    public void Create_ShouldCreateAndSaveContactToDatabase_ReturnContact()
    {
        // Arrange
        IContactRepository contactRepository = new ContactRepository( _context );
        var contactEntity = new ContactEntity { FirstName = "Anna", LastName = "Ozmehak", Email = "anna.ozmehak@gmail.com" };

        // Act
        var result = contactRepository.Create(contactEntity);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void Create_ShouldNotSaveContactToTheDatabase_ReturnNull()
    {
        // Arrange
        IContactRepository contactRepository = new ContactRepository(_context);
        var contactEntity = new ContactEntity();

        // Act
        var result = contactRepository.Create(contactEntity);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetAll_ShouldGetAllContacts_ReturnIEnumerableOfTypeContactEntity()
    {
        // Arrange
        IContactRepository contactRepository = new ContactRepository(_context);
        var contactEntity = new ContactEntity { FirstName = "Anna", LastName = "Ozmehak", Email = "anna.ozmehak@gmail.com" };
        contactRepository.Create(contactEntity);

        // Act
        var result = contactRepository.GetAll();

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<ContactEntity>>(result);
    }

    [Fact]
    public void GetAll_WhenNoContactsExist_ShouldReturnEmptyCollection()
    {
        // Arrange
        IContactRepository contactRepository = new ContactRepository(_context);

        // Act
        var result = contactRepository.GetAll();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void GetOneByEmail_ShouldFindOneContactByEmail_ReturnOneContact()
    {
        // Arrange
        IContactRepository contactRepository = new ContactRepository(_context);
        var contactEntity = new ContactEntity { FirstName = "Anna", LastName = "Ozmehak", Email = "anna.ozmehak@gmail.com" };
        contactRepository.Create(contactEntity);

        // Act
        var result = contactRepository.GetOne(x => x.Email == contactEntity.Email);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(contactEntity.FirstName, result.FirstName);
    }

    [Fact]
    public void GetOneByEmail_ShouldNotFindOneContactByEmail_ReturnNull()
    {
        // Arrange
        IContactRepository contactRepository = new ContactRepository(_context);
        var contactEntity = new ContactEntity { FirstName = "Anna", LastName = "Ozmehak", Email = "anna.ozmehak@gmail.com" };

        // Act
        var result = contactRepository.GetOne(x => x.Email == contactEntity.Email);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Delete_ShouldRemoveOneContact_ReturnTrue()
    {
        // Arrange
        IContactRepository contactRepository = new ContactRepository(_context);
        var contactEntity = new ContactEntity { FirstName = "Anna", LastName = "Ozmehak", Email = "anna.ozmehak@gmail.com" };
        contactRepository.Create(contactEntity);

        // Act
        var result = contactRepository.Delete(x => x.Email == contactEntity.Email);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Delete_ShouldNotFindContactAndRemoveIt_ReturnFalse()
    {
        // Arrange
        IContactRepository contactRepository = new ContactRepository(_context);
        var contactEntity = new ContactEntity { FirstName = "Anna", LastName = "Ozmehak", Email = "anna.ozmehak@gmail.com" };

        // Act
        var result = contactRepository.Delete(x => x.Email == contactEntity.Email);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Update_ShouldUpdateExistingContactEntity_ReturnUpdatedContactEntity()
    {
        // Arrange
        IContactRepository contactRepository = new ContactRepository(_context);
        var contactEntity = new ContactEntity { FirstName = "Anna", LastName = "Ozmehak", Email = "anna.ozmehak@gmail.com" };
        contactRepository.Create(contactEntity);

        // Act
        contactEntity.FirstName = "Calle";
        contactEntity.Email = "ozmehak.calle@gmail.com";
        var result = contactRepository.Update(contactEntity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(contactEntity.FirstName, result.FirstName);
        Assert.Equal(contactEntity.Email, result.Email);
    }

    [Fact]
    public void Exists_ShouldCheckIfContactExists_ReturnTrueIfContactExists()
    {
        // Arrange
        IContactRepository contactRepository = new ContactRepository(_context);
        var contactEntity = new ContactEntity { FirstName = "Anna", LastName = "Ozmehak", Email = "anna.ozmehak@gmail.com" };
        contactRepository.Create(contactEntity);

        // Act
        bool exists = contactRepository.Exists(x => x.Id == contactEntity.Id);

        // Assert
        Assert.True(exists);
    }

    [Fact]
    public void Exists_ShouldReturnFalse_WhenContactDoesNotExist()
    {
        // Arrange
        IContactRepository contactRepository = new ContactRepository(_context);
        int nonExistentContactId = 999;

        // Act
        bool exists = contactRepository.Exists(x => x.Id == nonExistentContactId);

        // Assert
        Assert.False(exists);
    }
}
