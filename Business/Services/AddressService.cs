using Business.Dtos;
using Business.Interfaces;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;

namespace Business.Services;

public class AddressService : IAddressService
{
    private readonly IAddressRepository _addressRepository;

    public AddressService(IAddressRepository addressRepository)
    {
        _addressRepository = addressRepository;
    }
    public AddressEntity Create(string streetName, string postalCode, string city)
    {
        var addressEntity = _addressRepository.GetOne(x => x.StreetName == streetName && x.PostalCode == postalCode && x.City == city);
        addressEntity ??= _addressRepository.Create(new AddressEntity { StreetName = streetName, PostalCode = postalCode, City = city });

        return addressEntity;
    }

    public bool Create(ContactDto contact)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<AddressEntity> GetAll()
    {
        var addresses = _addressRepository.GetAll();
        return addresses;
    }

    public AddressEntity GetOne(string streetName)
    {
        var addressEntity = _addressRepository.GetOne(x => x.StreetName == streetName);
        return addressEntity;
    }

    public void Remove(int id)
    {
        _addressRepository.Delete(x => x.Id == id);
    }

    public void Remove(string email)
    {
        throw new NotImplementedException();
    }

    //public AddressEntity Update(AddressEntity addressEntity)
    //{
        //var updatedAddressEntity = _addressRepository.Update(x => x.addressEntity.Id, addressEntity);
        //return updatedAddressEntity;
    //}

    public void Update(ContactDto contact)
    {
        throw new NotImplementedException();
    }

    IEnumerable<ContactDto> IAddressService.GetAll()
    {
        throw new NotImplementedException();
    }

    ContactDto IAddressService.GetOne(string email)
    {
        throw new NotImplementedException();
    }
}
