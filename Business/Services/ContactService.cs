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
            var addressEntity = _addressRepository.GetOne(x => x.StreetName == contact.StreetName && x.StreetNumber == contact.StreetNumber && x.PostalCode == contact.PostalCode && x.City == contact.City) ??
            _addressRepository.Create(new AddressEntity { StreetName = contact.StreetName, StreetNumber = contact.StreetNumber, PostalCode = contact.PostalCode, City = contact.City });

            if (addressEntity == null)
            {
                throw new Exception("Failed to retrieve or create AddressEntity");
            }

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

    public bool Update(ContactDto updatedContactDto)
    {
        try
        {
            var contactEntity = _contactRepository.GetOne(c => c.Email == updatedContactDto.Email);
            if (contactEntity == null)
            {
                return false;
            }

            var addressEntity = _addressRepository.GetOne(x => x.StreetName == updatedContactDto.StreetName && x.StreetNumber == updatedContactDto.StreetNumber && x.PostalCode == updatedContactDto.PostalCode && x.City == updatedContactDto.City);
            if (addressEntity == null)
            {
                var newAddress = new AddressEntity
                {
                    StreetName = updatedContactDto.StreetName,
                    StreetNumber = updatedContactDto.StreetNumber,
                    PostalCode = updatedContactDto.PostalCode,
                    City = updatedContactDto.City
                };
                _addressRepository.Create(newAddress);
                addressEntity = newAddress;
            }

            var phoneNumberEntity = _phoneNumberRepository.GetOne(p => p.PhoneNumber == updatedContactDto.PhoneNumber);
            if (phoneNumberEntity == null)
            {
                phoneNumberEntity = new PhoneNumberEntity { PhoneNumber = updatedContactDto.PhoneNumber };
                _phoneNumberRepository.Create(phoneNumberEntity);
            }

            contactEntity.FirstName = updatedContactDto.FirstName;
            contactEntity.LastName = updatedContactDto.LastName;
            contactEntity.Email = updatedContactDto.Email;
            contactEntity.AddressId = addressEntity.Id;
            //contactEntity.Address.StreetName = updatedContactDto.StreetName;
            //contactEntity.Address.StreetNumber = updatedContactDto.StreetNumber;
            //contactEntity.Address.PostalCode = updatedContactDto.PostalCode;
            //contactEntity.Address.City = updatedContactDto.City;
            contactEntity.PhoneNumberId = phoneNumberEntity.Id;
            contactEntity.PhoneNumber.PhoneNumber = phoneNumberEntity.PhoneNumber;

            _contactRepository.Update(contactEntity);
            return true;
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return false;
    }

    public bool Remove(string email)
    {
        try
        {
            var contact = _contactRepository.GetOne(x => x.Email == email);

            if (contact != null)
            {
                _contactRepository.Delete(x => x.Email == email);
                return true;
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return false;
    }
}
