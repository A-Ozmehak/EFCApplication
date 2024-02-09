using Business.Dtos;

namespace Business.Interfaces;

public interface IContactService
{
    /// <summary>
    /// Creates a new contact. If address or phone number does not exist, it creates them as well.
    /// </summary>
    /// <param name="contact">The contact data object that contains the details of the contact to be created</param>
    /// <returns>Returns ContactDto if the contact is created, otherwise null</returns>
    ContactDto CreateContact(ContactDto contact);

    /// <summary>
    /// Gets all contacts
    /// </summary>
    /// <returns>Returns a IEnumerable list of contacts</returns>
    IEnumerable<ContactDto> GetAll();

   /// <summary>
   /// Gets the contact based on the Id
   /// </summary>
   /// <param name="contact">The contact being shown</param>
   /// <returns>Return ContactDto otherwise null</returns>
    ContactDto GetOne(ContactDto contact);

   /// <summary>
   /// Updates a contact based on the Id
   /// </summary>
   /// <param name="contact">The contact being updated</param>
   /// <returns>Returns the contactEntity otherwise null</returns>
    ContactDto Update(ContactDto contact);

    /// <summary>
    /// Removes a contact by the email provided, also removed the address and phone number if it doesn't exist on another contact
    /// </summary>
    /// <param name="email">The email of the contact being removed</param>
    /// <returns>Returns true if the contact is removed, otherwise false</returns>
    bool Remove(string email);
}
