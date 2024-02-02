using Infrastructure.Entities;

namespace Infrastructure.Interfaces;

public interface IContactRepository : IRepository<ContactEntity>
{
    /// <summary>
    /// Gets a contact by email and includes Address and PhoneNumber
    /// </summary>
    /// <param name="email">The email of the contact</param>
    /// <returns>Returns the contact</returns>
    ContactEntity GetOneByEmail(string email);
}
