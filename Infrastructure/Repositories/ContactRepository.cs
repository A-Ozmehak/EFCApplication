using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
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

    public ContactEntity GetOneById(Expression<Func<ContactEntity, bool>> predicate)
    {
        try
        {
            var result = _context.Set<ContactEntity>()
            .Include(contact => contact.Address)
            .Include(contact => contact.PhoneNumber)
            .FirstOrDefault(predicate);

            return result!;
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }
}
