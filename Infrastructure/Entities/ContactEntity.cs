using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities;

public class ContactEntity
{

    [Key]
    public int Id { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(50)")]
    public string FirstName { get; set; } = null!;

    [Required]
    [Column(TypeName = "nvarchar(50)")]
    public string LastName { get; set; } = null!;

    [Required]
    [Column(TypeName = "varchar(100)")]
    public string Email { get; set; } = null!;

    public int AddressId { get; set; }

    public AddressEntity? Address { get; set; }

    [Required]
    public int PhoneNumberId { get; set; }
    public PhoneNumberEntity PhoneNumber { get; set; } = null!;
}
