using Business.Dtos;

namespace Business.Interfaces;

public interface IPhoneNumberService
{
    bool Create(ContactDto contact);
    IEnumerable<ContactDto> GetAll();
    ContactDto GetOne(string email);
    void Update(ContactDto contact);
    void Remove(string email);
}
