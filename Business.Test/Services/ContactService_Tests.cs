using Business.Dtos;
using Business.Interfaces;
using Business.Services;
using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Moq;
using System.Linq.Expressions;

namespace Business.Test.Services;

public class ContactService_Tests
{
    private readonly ContactContext _context =
       new ContactContext(new DbContextOptionsBuilder<ContactContext>()
           .UseInMemoryDatabase($"{Guid.NewGuid()}")
           .Options);

    [Fact]
    public void CreateContact_ShouldCreateAndSaveContact_ReturnContact()
    {
        // Arrange
        var mockContactRepository = new Mock<IContactRepository>();
        var mockAddressRepository = new Mock<IAddressRepository>();
        var mockPhoneNumberRepository = new Mock<IPhoneNumberRepository>();
        IContactService contactService = new ContactService(mockContactRepository.Object, mockAddressRepository.Object, mockPhoneNumberRepository.Object);

        var contactDtoToCreate = new ContactDto { FirstName = "Test", LastName = "User", Email = "test.user@example.com", StreetName = "Test Street", StreetNumber = "123", PostalCode = "12345", City = "Test City", PhoneNumber = "1234567890" };
        
        mockAddressRepository.Setup(x => x.Create(It.IsAny<AddressEntity>())).Returns(new AddressEntity());
        mockPhoneNumberRepository.Setup(x => x.Create(It.IsAny<PhoneNumberEntity>())).Returns(new PhoneNumberEntity());
        mockContactRepository.Setup(x => x.Create(It.IsAny<ContactEntity>())).Returns(new ContactEntity());

        // Act
        var result = contactService.CreateContact(contactDtoToCreate);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void CreateContact_ShouldNotCreateAndSaveContact_ReturnFalse()
    {
        // Arrange
        var mockContactRepository = new Mock<IContactRepository>();
        var mockAddressRepository = new Mock<IAddressRepository>();
        var mockPhoneNumberRepository = new Mock<IPhoneNumberRepository>();
        IContactService contactService = new ContactService(mockContactRepository.Object, mockAddressRepository.Object, mockPhoneNumberRepository.Object);

        var contactDto = new ContactDto { FirstName = "Test", LastName = "User", Email = "test.user@example.com", StreetName = "Test Street", StreetNumber = "123", PostalCode = "12345", City = "Test City", PhoneNumber = "1234567890" };

        // Act
        var result = contactService.CreateContact(contactDto);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetAll_ShouldGetAllContacts_ReturnIEnumerableOfTypeContacts()
    {
        // Arrange
        var mockContactRepository = new Mock<IContactRepository>();
        var mockAddressRepository = new Mock<IAddressRepository>();
        var mockPhoneNumberRepository = new Mock<IPhoneNumberRepository>();
        IContactService contactService = new ContactService(mockContactRepository.Object, mockAddressRepository.Object, mockPhoneNumberRepository.Object);

        var contacts = new List<ContactEntity>
        {
            new ContactEntity { Id = 1, FirstName = "Test", LastName = "User", Email = "test.user@example.com", Address = new AddressEntity { StreetName = "Gustaf dalensgatan", StreetNumber = "24", PostalCode = "12345", City = "Goteborg" }, PhoneNumber = new PhoneNumberEntity { PhoneNumber = "1234567894" }},
            new ContactEntity { Id = 2, FirstName = "Test1", LastName = "User1", Email = "test1.user@example.com", Address = new AddressEntity { StreetName = "Test Street 1", StreetNumber = "123", PostalCode = "12345", City = "Test City 1" }, PhoneNumber = new PhoneNumberEntity { PhoneNumber = "1234567890" }},
        };

        mockContactRepository.Setup(x => x.GetAll()).Returns(contacts);

        // Act
        var result = contactService.GetAll();

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Contains(result, r => r.FirstName == "Test" && r.LastName == "User" && r.StreetName == "Gustaf dalensgatan" && r.PhoneNumber == "1234567894");
        Assert.Contains(result, r => r.FirstName == "Test1" && r.LastName == "User1" && r.StreetName == "Test Street 1" && r.PhoneNumber == "1234567890");
    }

    [Fact]
    public void GetAll_ShouldGetNoContacts_ReturnEmptyIEnumerable()
    {
        // Arrange
        var mockContactRepository = new Mock<IContactRepository>();
        var mockAddressRepository = new Mock<IAddressRepository>();
        var mockPhoneNumberRepository = new Mock<IPhoneNumberRepository>();
        IContactService contactService = new ContactService(mockContactRepository.Object, mockAddressRepository.Object, mockPhoneNumberRepository.Object);

        var contacts = new List<ContactEntity>();

        mockContactRepository.Setup(x => x.GetAll()).Returns(contacts);

        // Act
        var result = contactService.GetAll();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void GetOne_ShouldGetOneContactById_ReturnOneContact()
    {
        // Arrange
        var mockContactRepository = new Mock<IContactRepository>();
        var mockAddressRepository = new Mock<IAddressRepository>();
        var mockPhoneNumberRepository = new Mock<IPhoneNumberRepository>();
        IContactService contactService = new ContactService(mockContactRepository.Object, mockAddressRepository.Object, mockPhoneNumberRepository.Object);

        var contactEntity = new ContactEntity { Id = 1, FirstName = "Test", LastName = "User", Email = "test.user@example.com", Address = new AddressEntity { StreetName = "Test Street", StreetNumber = "123", PostalCode = "12345", City = "Test City" }, PhoneNumber = new PhoneNumberEntity { PhoneNumber = "1234567890" } };

        mockContactRepository.Setup(x => x.GetOneById(It.IsAny<Expression<Func<ContactEntity, bool>>>()))
            .Returns(contactEntity);

        // Act
        var result = contactService.GetOne(contactEntity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(contactEntity.FirstName, result.FirstName);
        Assert.Equal(contactEntity.LastName, result.LastName);
        Assert.Equal(contactEntity.Email, result.Email);
    }

    [Fact]
    public void GetOne_ShouldNotFindOneContactById_ReturnNull()
    {
        // Arrange
        var mockContactRepository = new Mock<IContactRepository>();
        var mockAddressRepository = new Mock<IAddressRepository>();
        var mockPhoneNumberRepository = new Mock<IPhoneNumberRepository>();
        IContactService contactService = new ContactService(mockContactRepository.Object, mockAddressRepository.Object, mockPhoneNumberRepository.Object);

        var contactToGet = new ContactDto { Id = 1 };

        mockContactRepository.Setup(repo => repo.GetOneById(It.IsAny<Expression<Func<ContactEntity, bool>>>()))
                   .Returns((ContactEntity)null);

        // Act
        var result = contactService.GetOne(contactToGet);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Update_ShouldUpdateContact_ReturnUpdatedContactEntity()
    {
        // Arrange
        var mockContactRepository = new Mock<IContactRepository>();
        var mockAddressRepository = new Mock<IAddressRepository>();
        var mockPhoneNumberRepository = new Mock<IPhoneNumberRepository>();
        IContactService contactService = new ContactService(mockContactRepository.Object, mockAddressRepository.Object, mockPhoneNumberRepository.Object);

        var contactEntity = new ContactEntity { FirstName = "Anna", LastName = "Ozmehak", Email = "anna.ozmehak@gmail.com", Address = new AddressEntity { StreetName = "Gustaf dalensgatan", StreetNumber = "24", PostalCode = "41724", City = "Goteborg" }, PhoneNumber = new PhoneNumberEntity { PhoneNumber = "0793555635" } };
        var contactDto = new ContactDto { FirstName = "Calle", LastName = "Ozmehak", Email = "ozmehak.calle@gmail.com", StreetName = "Gustaf dalensgatan", StreetNumber = "24", PostalCode = "41724", City = "Goteborg", PhoneNumber = "0735687334" };

        mockContactRepository.Setup(x => x.GetOne(It.IsAny<Expression<Func<ContactEntity, bool>>>())).Returns(contactEntity);
        mockContactRepository.Setup(x => x.Update(It.IsAny<ContactEntity>())).Callback<ContactEntity>(c => contactEntity = c);

        // Act
        var result = contactService.Update(contactDto);

        // Assert
        Assert.Equal(contactDto.FirstName, contactEntity.FirstName);
        Assert.Equal(contactDto.Email, contactEntity.Email);
        Assert.Equal(contactDto.PhoneNumber, contactEntity.PhoneNumber.PhoneNumber);
    }

    [Fact]
    public void Update_ShouldNotUpdateContact_WhenContactDoesNotExist()
    {
        // Arrange
        var mockContactRepository = new Mock<IContactRepository>();
        var mockAddressRepository = new Mock<IAddressRepository>();
        var mockPhoneNumberRepository = new Mock<IPhoneNumberRepository>();
        IContactService contactService = new ContactService(mockContactRepository.Object, mockAddressRepository.Object, mockPhoneNumberRepository.Object);

        var nonExistentContactDto = new ContactDto { FirstName = "Non", LastName = "Existent", Email = "non.existent.user@example.com", StreetName = "Non Existent Street", StreetNumber = "123", PostalCode = "12345", City = "Non Existent City", PhoneNumber = "1234567890" };

        mockContactRepository.Setup(x => x.GetOne(It.IsAny<Expression<Func<ContactEntity, bool>>>())).Returns((ContactEntity)null);

        // Act
        var result = contactService.Update(nonExistentContactDto);

        // Assert
        Assert.Null(result);
        mockContactRepository.Verify(x => x.Update(It.IsAny<ContactEntity>()), Times.Never);
    }

    [Fact]
    public void Remove_ShouldRemoveContactByEmail_ReturnTrue()
    {
        // Arrange
        var mockContactRepository = new Mock<IContactRepository>();
        var mockAddressRepository = new Mock<IAddressRepository>();
        var mockPhoneNumberRepository = new Mock<IPhoneNumberRepository>();
        IContactService contactService = new ContactService(mockContactRepository.Object, mockAddressRepository.Object, mockPhoneNumberRepository.Object);

        var contactEntity = new ContactEntity { Id = 1, FirstName = "Test", LastName = "User", Email = "test.user@example.com", Address = new AddressEntity { StreetName = "Test Street", StreetNumber = "123", PostalCode = "12345", City = "Test City" }, PhoneNumber = new PhoneNumberEntity { PhoneNumber = "1234567890" } };

        mockContactRepository.Setup(x => x.GetOne(It.IsAny<Expression<Func<ContactEntity, bool>>>())).Returns(contactEntity);
        mockContactRepository.Setup(x => x.Delete(It.Is<Expression<Func<ContactEntity, bool>>>(predicate => predicate.Compile()(contactEntity)))).Returns(true);

        // Act
        var result = contactService.Remove(contactEntity.Email);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Remove_ShouldNotFindContactAndRemoveIt_ReturnFalse()
    {
        // Arrange
        var mockContactRepository = new Mock<IContactRepository>();
        var mockAddressRepository = new Mock<IAddressRepository>();
        var mockPhoneNumberRepository = new Mock<IPhoneNumberRepository>();
        IContactService contactService = new ContactService(mockContactRepository.Object, mockAddressRepository.Object, mockPhoneNumberRepository.Object);

        var nonExistentEmail = "non.existent.user@example.com";

        mockContactRepository.Setup(x => x.GetOne(It.IsAny<Expression<Func<ContactEntity, bool>>>())).Returns((ContactEntity)null);

        // Act
        contactService.Remove(nonExistentEmail);

        // Assert
        mockContactRepository.Verify(x => x.Delete(It.IsAny<Expression<Func<ContactEntity, bool>>>()), Times.Never);
    }
}
