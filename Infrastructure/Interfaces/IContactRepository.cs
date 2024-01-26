using Infrastructure.Entities;

namespace Infrastructure.Interfaces;

public interface IContactRepository : IRepository<ContactEntity>
{
    ContactEntity GetOneByEmail(string email);
}
