using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories;

public class ContactRepository : Repository<ContactEntity, ContactContext>, IContactRepository
{
    private readonly ContactContext _context;

    public ContactRepository(ContactContext context) : base(context)
    {
        _context = context;
    }
}
