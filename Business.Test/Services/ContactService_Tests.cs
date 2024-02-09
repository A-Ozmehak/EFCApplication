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
        IContactRepository contactRepository = new ContactRepository(_context);
        IAddressRepository addressRepository = new AddressRepository(_context);
        IPhoneNumberRepository phoneNumberRepository = new PhoneNumberRepository(_context);
        IContactService contactService = new ContactService(contactRepository, addressRepository, phoneNumberRepository);

        var contactDto = new ContactDto { FirstName = "Anna", LastName = "Ozmehak", Email = "anna.ozmehak@gmail.com", PhoneNumber = "0793555635", StreetName = "Gustaf dalensgatan", StreetNumber = "24", PostalCode = "41724", City = "Goteborg" };

        // Act
        var result = contactService.CreateContact(contactDto);

        // Assert
        Assert.NotNull(result);
        var createdContact = _context.Contacts.FirstOrDefault(c => c.FirstName == contactDto.FirstName);
        Assert.NotNull(createdContact);
        Assert.Equal(contactDto.Email, createdContact.Email);
        Assert.Equal(contactDto.PhoneNumber, createdContact.PhoneNumber.PhoneNumber);
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
    public void GetAll_ShouldGetAllContacts_ReturnContacts()
    {
        // Arrange
        var firstContact = new ContactEntity { FirstName = "Anna", LastName = "Ozmehak", Email = "anna.ozmehak@gmail.com", PhoneNumber = new PhoneNumberEntity { PhoneNumber = "0793555635" },  Address = new AddressEntity { StreetName = "Gustaf dalensgatan", StreetNumber = "24", PostalCode = "41724", City = "Goteborg" } };
        var secondContact = new ContactEntity { FirstName = "Calle", LastName = "Ozmehak", Email = "ozmehak.calle@gmail.com", PhoneNumber = new PhoneNumberEntity { PhoneNumber = "0793555634" }, Address = new AddressEntity { StreetName = "Gustaf dalensgatan", StreetNumber = "24", PostalCode = "41724", City = "Goteborg" } };
        _context.Contacts.AddRange(firstContact, secondContact);
        _context.SaveChanges();

        IContactRepository contactRepository = new ContactRepository(_context);
        IAddressRepository addressRepository = new AddressRepository(_context);
        IPhoneNumberRepository phoneNumberRepository = new PhoneNumberRepository(_context);
        var contactService = new ContactService(contactRepository, addressRepository, phoneNumberRepository);

        // Act
        var result = contactService.GetAll().ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, r => r.FirstName == firstContact.FirstName && r.Email == firstContact.Email);
        Assert.Contains(result, r => r.FirstName == secondContact.FirstName && r.Email == secondContact.Email);
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
    public void GetOne_IfContactExists_ReturnOneContact()
    {
        // Arrange
        var contact = new ContactEntity { FirstName = "Anna", LastName = "Ozmehak", Email = "anna.ozmehak@gmail.com", PhoneNumber = new PhoneNumberEntity { PhoneNumber = "0793555635" }, Address = new AddressEntity { StreetName = "Gustaf dalensgatan", StreetNumber = "24", PostalCode = "41724", City = "Goteborg" } };
        _context.Contacts.Add(contact);
        _context.SaveChanges();

        IContactRepository contactRepository = new ContactRepository(_context);
        IAddressRepository addressRepository = new AddressRepository(_context);
        IPhoneNumberRepository phoneNumberRepository = new PhoneNumberRepository(_context);
        var contactService = new ContactService(contactRepository, addressRepository, phoneNumberRepository);

        var contactDto = new ContactDto { Id = contact.Id };

        // Act
        var result = contactService.GetOne(contactDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(contact.FirstName, result.FirstName);
        Assert.Equal(contact.Email, result.Email);
        Assert.Equal(contact.LastName, result.LastName);
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
    public void Update_ShouldUpdateContact_ReturnUpdated()
    {
        // Arrange
        var contact = new ContactEntity { FirstName = "Anna", LastName = "Ozmehak", Email = "anna.ozmehak@gmail.com", PhoneNumber = new PhoneNumberEntity { PhoneNumber = "0793555635" }, Address = new AddressEntity { StreetName = "Gustaf dalensgatan", StreetNumber = "24", PostalCode = "41724", City = "Goteborg" } };
        _context.Contacts.Add(contact);
        _context.SaveChanges();

        IContactRepository contactRepository = new ContactRepository(_context);
        IAddressRepository addressRepository = new AddressRepository(_context);
        IPhoneNumberRepository phoneNumberRepository = new PhoneNumberRepository(_context);
        IContactService contactService = new ContactService(contactRepository, addressRepository, phoneNumberRepository);

        var updatedContactDto = new ContactDto { FirstName = "Calle", LastName = "Ozmehak", Email = "ozmehak.calle@gmail.com", StreetName = "Gustaf dalensgatan", StreetNumber = "24", PostalCode = "41724", City = "Goteborg", PhoneNumber = "0793555635" };
        
        // Act
        var result = contactService.Update(updatedContactDto);
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
    public void Remove_ShouldRemoveContact_ReturnTrue()
    {
        // Arrange
        var contact = new ContactEntity { FirstName = "Anna", LastName = "Ozmehak", Email = "anna.ozmehak@gmail.com", PhoneNumber = new PhoneNumberEntity { PhoneNumber = "0793555635" }, Address = new AddressEntity { StreetName = "Gustaf dalensgatan", StreetNumber = "24", PostalCode = "41724", City = "Goteborg" } };
        _context.Contacts.Add(contact);
        _context.SaveChanges();

        IContactRepository contactRepository = new ContactRepository(_context);
        IAddressRepository addressRepository = new AddressRepository(_context);
        IPhoneNumberRepository phoneNumberRepository = new PhoneNumberRepository(_context);
        IContactService contactService = new ContactService(contactRepository, addressRepository, phoneNumberRepository);

        // Act
        var result = contactService.Remove(contact.Email);

        // Assert
        Assert.True(result);
        Assert.False(_context.Contacts.Any(c => c.Id == contact.Id));
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
