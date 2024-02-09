using Infrastructure.Entities;
using System.Linq.Expressions;

namespace Infrastructure.Interfaces;

public interface IContactRepository : IRepository<ContactEntity>
{
    /// <summary>
    /// Gets the contact by id and includes the address and phone number
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition</param>
    /// <returns>Returns the contact otherwise null</returns>
    ContactEntity GetOneById(Expression<Func<ContactEntity, bool>> predicate);
}
