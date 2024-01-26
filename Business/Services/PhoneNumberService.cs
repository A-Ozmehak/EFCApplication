using Business.Dtos;
using Business.Interfaces;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using System.Diagnostics;
using System.Net;

namespace Business.Services;

public class PhoneNumberService : IPhoneNumberService
{
    private readonly IPhoneNumberRepository _phoneNumberRepository;

    public PhoneNumberService(IPhoneNumberRepository phoneNumberRepository)
    {
        _phoneNumberRepository = phoneNumberRepository;
    }
    public bool Create(ContactDto contact)
    {
        try
        {
            if (!_phoneNumberRepository.Exists(x => x.PhoneNumber == contact.PhoneNumber))
            {
                var phoneNumberEntity = new PhoneNumberEntity
                {
                    PhoneNumber = contact.PhoneNumber
                };

                var result = _phoneNumberRepository.Create(phoneNumberEntity);
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
        var numbers = new List<ContactDto>();

        try
        {
            var result = _phoneNumberRepository.GetAll();

            foreach (var number in result)
            {
                numbers.Add(new ContactDto
                {
                    PhoneNumber = number.PhoneNumber
                });
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return numbers;
    }

    public ContactDto GetOne(string phoneNumber)
    {
        try
        {
            var number = _phoneNumberRepository.GetOne(x => x.PhoneNumber == phoneNumber);
            if (number != null)
            {
                return new ContactDto
                {
                    PhoneNumber = number.PhoneNumber
                };
            }

        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }

    public void Remove(string phoneNumber)
    {
        try
        {
            var number = _phoneNumberRepository.GetOne(x => x.PhoneNumber == phoneNumber);

            if (phoneNumber != null)
            {
                _phoneNumberRepository.Delete(x => x.PhoneNumber == phoneNumber);
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
    }

    public void Update(ContactDto contact)
    {
        try
        {
            var existingPhoneNumber = _phoneNumberRepository.GetOne(number => number.PhoneNumber == contact.PhoneNumber);
            if (existingPhoneNumber != null)
            {
                existingPhoneNumber.PhoneNumber = contact.PhoneNumber;
           
                _phoneNumberRepository.Update(existingPhoneNumber);
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
    }
}
