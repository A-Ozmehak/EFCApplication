using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class ContactRepository : Repository<ContactEntity, ContactContext>, IContactRepository
{
    private readonly ContactContext _context;

    public ContactRepository(ContactContext context) : base(context)
    {
        _context = context;
    }

    public override List<ContactEntity> GetAll()
    {
        return _context.Contacts
            .Include(contact => contact.Address)
            .Include(contact => contact.PhoneNumber)
            .ToList();
    }

    public ContactEntity GetOneByEmail(string email)
    {
        return _context.Contacts
            .Include(contact => contact.Address)
            .Include(contact => contact.PhoneNumber)
            .SingleOrDefault(contact => contact.Email == email)!;
    }
}
