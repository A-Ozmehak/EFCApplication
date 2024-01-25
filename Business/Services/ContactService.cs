using Business.Dtos;
using Business.Interfaces;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Net;

namespace Business.Services;

public class ContactService(IContactRepository contactRepository) : IContactService
{
    private readonly IContactRepository _contactRepository = contactRepository;

    public bool CreateContact(ContactDto contact)
    {
        try
        {
            if (!_contactRepository.Exists(x => x.Email == contact.Email))
            {
                var contactEntity = new ContactEntity
                {
                    FirstName = contact.FirstName,
                    LastName = contact.LastName,
                    Email = contact.Email,
                    Address = contact.Address,
                    PostalCode = contact.PostalCode,
                    City = contact.City,
                    PhoneNumber = contact.PhoneNumber
                };

                var result = _contactRepository.Create(contactEntity);
                if (result != null)
                {
                    return true;
                }
            }
        }
        catch(Exception ex) { Debug.WriteLine(ex.Message); }
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
                    Address = contact.Address,
                    PostalCode = contact.PostalCode,
                    City = contact.City,
                    PhoneNumber = contact.PhoneNumber
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
           var contact = _contactRepository.GetOne(x => x.Email == email);
            if (contact != null)
            {
                return new ContactDto
                {
                    FirstName = contact.FirstName,
                    LastName = contact.LastName,
                    Email = contact.Email,
                    Address = contact.Address,
                    PostalCode = contact.PostalCode,
                    City = contact.City,
                    PhoneNumber = contact.PhoneNumber
                };
            }
            
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }

    public void Update(ContactDto contact)
    {
        try
        {
            var existingContact = _contactRepository.GetOne(x => x.Email == contact.Email);
            if (existingContact != null)
            {
                existingContact.FirstName = contact.FirstName;
                existingContact.LastName = contact.LastName;
                existingContact.Email = contact.Email;
                existingContact.Address = contact.Address;
                existingContact.PostalCode = contact.PostalCode;
                existingContact.City = contact.City;
                existingContact.PhoneNumber = contact.PhoneNumber;

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
