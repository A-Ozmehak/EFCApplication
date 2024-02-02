using Business.Dtos;

namespace Business.Interfaces;

public interface IContactService
{
    /// <summary>
    /// Creates a new contact. If address or phone number does not exist, it creates them as well.
    /// </summary>
    /// <param name="contact">The contact data object that contains the details of the contact to be created</param>
    /// <returns>Returns true if the contact is created, otherwise false</returns>
    bool CreateContact(ContactDto contact);

    /// <summary>
    /// Gets all contacts
    /// </summary>
    /// <returns>Returns a IEnumerable list of contacts</returns>
    IEnumerable<ContactDto> GetAll();

    /// <summary>
    /// Gets one contact by the email provided.
    /// </summary>
    /// <param name="email">The email of the contact being shown</param>
    /// <returns>Returns the contact, otherwise null</returns>
    ContactDto GetOne(string email);

    /// <summary>
    /// Updates the contact by the email provided.
    /// </summary>
    /// <param name="contact">The contact being updated</param>
    /// <returns>Returns true if the contact is updated, otherwise false</returns>
    bool Update(ContactDto contact);

    /// <summary>
    /// Removes a contact by the email provided.
    /// </summary>
    /// <param name="email">The email of the contact being removed</param>
    /// <returns>Returns true if the contact is removed, otherwise false</returns>
    bool Remove(string email);
}
