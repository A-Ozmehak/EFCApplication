using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Business.Test.Services;

public class ContactService_Tests
{
    private readonly ContactContext _context =
       new ContactContext(new DbContextOptionsBuilder<ContactContext>()
           .UseInMemoryDatabase($"{Guid.NewGuid()}")
           .Options);

    [Fact]
    public void CreateContact_ShouldCreateContactToRepository_ReturnContact()
    {
        // Arrange
        IContactRepository contactRepository = new ContactRepository(_context);
        var addressEntity = new AddressEntity { StreetName = "Gustaf dalensgatan", StreetNumber = "24", PostalCode = "41724", City = "Goteborg" };
        var phoneNumberEntity = new PhoneNumberEntity { PhoneNumber = "0793555635" };
        var contactEntity = new ContactEntity { FirstName = "Anna", LastName = "Ozmehak", Email = "anna.ozmehak@gmail.com", Address = addressEntity, PhoneNumber = phoneNumberEntity };

        // Act
        var result = contactRepository.Create(contactEntity);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void CreateContact_ShouldNotCreateContactToRepository_ReturnNull()
    {
        // Arrange
        IContactRepository contactRepository = new ContactRepository(_context);
        var addressEntity = new AddressEntity();
        var phoneNumberEntity = new PhoneNumberEntity();
        var contactEntity = new ContactEntity();

        // Act
        var result = contactRepository.Create(contactEntity);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetAll_ShouldGetAllContacts_ReturnIEnumerableOfTypeContacts()
    {
        // Arrange
        IContactRepository contactRepository = new ContactRepository(_context);
        var addressEntity = new AddressEntity { StreetName = "Gustaf dalensgatan", StreetNumber = "24", PostalCode = "41724", City = "Goteborg" };
        var phoneNumberEntity = new PhoneNumberEntity { PhoneNumber = "0793555635" };
        var contactEntity = new ContactEntity { FirstName = "Anna", LastName = "Ozmehak", Email = "anna.ozmehak@gmail.com", Address = addressEntity, PhoneNumber = phoneNumberEntity };
        contactRepository.Create(contactEntity);

        // Act
        var result = contactRepository.GetAll();

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<ContactEntity>>(result);
    }

    [Fact]
    public void GetOne_ShouldGetOneContactByEmail_ReturnOneContact()
    {
        // Arrange
        IContactRepository contactRepository = new ContactRepository(_context);
        var addressEntity = new AddressEntity { StreetName = "Gustaf dalensgatan", StreetNumber = "24", PostalCode = "41724", City = "Goteborg" };
        var phoneNumberEntity = new PhoneNumberEntity { PhoneNumber = "0793555635" };
        var contactEntity = new ContactEntity { FirstName = "Anna", LastName = "Ozmehak", Email = "anna.ozmehak@gmail.com", Address = addressEntity, PhoneNumber = phoneNumberEntity };
        contactRepository.Create(contactEntity);

        // Act
        var result = contactRepository.GetOne(x => x.Email == contactEntity.Email);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(contactEntity.FirstName, result.FirstName);
    }

    [Fact]
    public void GetOne_ShouldNotFindOneContactByEmail_ReturnNull()
    {
        // Arrange
        IContactRepository contactRepository = new ContactRepository(_context);
        var addressEntity = new AddressEntity { StreetName = "Gustaf dalensgatan", StreetNumber = "24", PostalCode = "41724", City = "Goteborg" };
        var phoneNumberEntity = new PhoneNumberEntity { PhoneNumber = "0793555635" };
        var contactEntity = new ContactEntity { FirstName = "Anna", LastName = "Ozmehak", Email = "anna.ozmehak@gmail.com", Address = addressEntity, PhoneNumber = phoneNumberEntity };

        // Act
        var result = contactRepository.GetOne(x => x.Email == contactEntity.Email);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Update_ShouldUpdateContactByEmail_ReturnTrue()
    {
        // Arrange
        IContactRepository contactRepository = new ContactRepository(_context);
        var addressEntity = new AddressEntity { StreetName = "Gustaf dalensgatan", StreetNumber = "24", PostalCode = "41724", City = "Goteborg" };
        var phoneNumberEntity = new PhoneNumberEntity { PhoneNumber = "0793555635" };
        var contactEntity = new ContactEntity { FirstName = "Anna", LastName = "Ozmehak", Email = "anna.ozmehak@gmail.com", Address = addressEntity, PhoneNumber = phoneNumberEntity };
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
    public void Remove_ShouldRemoveContactByEmail_ReturnTrue()
    {
        // Arrange
        IContactRepository contactRepository = new ContactRepository(_context);
        var addressEntity = new AddressEntity { StreetName = "Gustaf dalensgatan", StreetNumber = "24", PostalCode = "41724", City = "Goteborg" };
        var phoneNumberEntity = new PhoneNumberEntity { PhoneNumber = "0793555635" };
        var contactEntity = new ContactEntity { FirstName = "Anna", LastName = "Ozmehak", Email = "anna.ozmehak@gmail.com", Address = addressEntity, PhoneNumber = phoneNumberEntity };
        contactRepository.Create(contactEntity);

        // Act
        var result = contactRepository.Delete(x => x.Email == contactEntity.Email);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Remove_ShouldNotFindContactAndRemoveIt_ReturnFalse()
    {
        // Arrange
        IContactRepository contactRepository = new ContactRepository(_context);
        var addressEntity = new AddressEntity { StreetName = "Gustaf dalensgatan", StreetNumber = "24", PostalCode = "41724", City = "Goteborg" };
        var phoneNumberEntity = new PhoneNumberEntity { PhoneNumber = "0793555635" };
        var contactEntity = new ContactEntity { FirstName = "Anna", LastName = "Ozmehak", Email = "anna.ozmehak@gmail.com", Address = addressEntity, PhoneNumber = phoneNumberEntity };

        // Act
        var result = contactRepository.Delete(x => x.Email == contactEntity.Email);

        // Assert
        Assert.False(result);
    }
}
