using Business.Dtos;
using Business.Interfaces;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using System.Diagnostics;

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

    public ContactDto GetOne(ContactDto contact)
    {
        try
        {
            if (_contactRepository.Exists(x => x.FirstName == contact.FirstName))
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
                    return contact;
                }

            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }

    public bool Delete(ContactDto contact)
    {
        try
        {
            return true;
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return false;
    }
}
