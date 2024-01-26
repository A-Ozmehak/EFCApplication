using Business.Dtos;
using Business.Interfaces;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Net;

namespace Business.Services;

public class ContactService(IContactRepository contactRepository, IAddressRepository addressRepository, IPhoneNumberRepository phoneNumberRepository) : IContactService
{
    private readonly IContactRepository _contactRepository = contactRepository;
    private readonly IAddressRepository _addressRepository = addressRepository;
    private readonly IPhoneNumberRepository _phoneNumberRepository = phoneNumberRepository;

    public bool CreateContact(ContactDto contact)
    {
        try
        {
            if (!_contactRepository.Exists(x => x.Email == contact.Email))
            {
                var addressEntity = _addressRepository.GetOne(x => x.StreetName == contact.StreetName);
                addressEntity ??= _addressRepository.Create(new AddressEntity { StreetName = contact.StreetName, StreetNumber = contact.StreetNumber, PostalCode = contact.PostalCode, City = contact.City });

                var phoneNumberEntity = _phoneNumberRepository.GetOne(x => x.PhoneNumber == contact.PhoneNumber);
                phoneNumberEntity ??= _phoneNumberRepository.Create(new PhoneNumberEntity { PhoneNumber = contact.PhoneNumber });

                var contactEntity = new ContactEntity
                {
                    FirstName = contact.FirstName,
                    LastName = contact.LastName,
                    Email = contact.Email,
                    AddressId = addressEntity.Id,
                    PhoneNumberId = phoneNumberEntity.Id                
                };

                var result = _contactRepository.Create(contactEntity);
                if (result != null)
                {
                    return true;
                }
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return false;
    }

    public IEnumerable<ContactDto> GetAll()
    {
        var contacts = new List<ContactDto>();

        try
        {
            var result = _contactRepository.GetAll();

            foreach (var contact in result)
            {
                contacts.Add(new ContactDto
                {
                    FirstName = contact.FirstName,
                    LastName = contact.LastName,
                    Email = contact.Email,
                    StreetName = contact.Address?.StreetName,
                    StreetNumber = contact.Address?.StreetNumber,
                    PostalCode = contact.Address?.PostalCode,
                    City = contact.Address?.City,
                    PhoneNumber = contact.PhoneNumber?.PhoneNumber
                });
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return contacts;
    }

    public ContactDto GetOne(string email)
    {
        try
        {
            var contact = _contactRepository.GetOneByEmail(email);
            if (contact != null)
            {
                return new ContactDto
                {
                    FirstName = contact.FirstName,
                    LastName = contact.LastName,
                    Email = contact.Email,
                    StreetName = contact.Address?.StreetName,
                    StreetNumber = contact.Address?.StreetNumber,
                    PostalCode = contact.Address?.PostalCode,
                    City = contact.Address?.City,
                    PhoneNumber = contact.PhoneNumber?.PhoneNumber
                };
            }

        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }

    public void Update(ContactDto updatedContactDto)
    {
        try
        {
            var existingContact = _contactRepository.GetOne(contact => contact.Email == updatedContactDto.Email);
            if (existingContact != null)
            {
                existingContact.FirstName = updatedContactDto.FirstName;
                existingContact.LastName = updatedContactDto.LastName;
                existingContact.Email = updatedContactDto.Email;
                existingContact.Address!.StreetName = updatedContactDto.StreetName;
                existingContact.Address.PostalCode = updatedContactDto.PostalCode;
                existingContact.Address.City = updatedContactDto.City;
                existingContact.PhoneNumber.PhoneNumber = updatedContactDto.PhoneNumber;

                _contactRepository.Update(existingContact);
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
    }

    public void Remove(string email)
    {
        try
        {
            var contact = _contactRepository.GetOne(x => x.Email == email);

            if (contact != null)
            {
                _contactRepository.Delete(x => x.Email == email);
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }

    }
}
