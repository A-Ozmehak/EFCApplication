using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contexts;

public class ContactContext(DbContextOptions<ContactContext> options) : DbContext(options)
{
    public virtual DbSet<ContactEntity> Contacts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ContactEntity>()
           .HasIndex(x => x.Email)
           .IsUnique();
    }
}
