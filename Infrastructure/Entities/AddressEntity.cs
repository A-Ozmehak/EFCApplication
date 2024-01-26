using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities;

public class AddressEntity
{
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "nvarchar(50)")]
    public string? StreetName { get; set; }

    [Column(TypeName = "nvarchar(10)")]
    public string? StreetNumber { get; set; }

    [Column(TypeName = "varchar(6)")]
    public string? PostalCode { get; set; }

    [Column(TypeName = "nvarchar(50)")]
    public string? City { get; set; }
}
