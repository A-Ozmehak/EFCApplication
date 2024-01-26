using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories;

public class PhoneNumberRepository : Repository<PhoneNumberEntity, ContactContext>, IPhoneNumberRepository
{
    private readonly ContactContext _context;

    public PhoneNumberRepository(ContactContext context) : base(context)
    {
        _context = context;
    }
}
