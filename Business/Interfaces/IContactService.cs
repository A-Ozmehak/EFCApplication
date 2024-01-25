using Business.Dtos;

namespace Business.Interfaces;

public interface IContactService
{
    bool CreateContact(ContactDto contact);
    IEnumerable<ContactDto> GetAll();
}
