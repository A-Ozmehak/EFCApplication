using Infrastructure.Entities;

namespace Business.Dtos;

public class ContactDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;
    public string? StreetName { get; set; }
    public string? StreetNumber { get; set; }
    public string? PostalCode { get; set; }
    public string? City { get; set; }
    public string PhoneNumber { get; set; } = null!;



    public static implicit operator ContactDto(ContactEntity entity)
    {
        var contactDto = new ContactDto
        {
            Id = entity.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Email = entity.Email,
            StreetName = entity.Address?.StreetName,
            StreetNumber = entity.Address?.StreetNumber,
            PostalCode = entity.Address?.PostalCode,
            City = entity.Address?.City,
            PhoneNumber = entity.PhoneNumber.PhoneNumber
        };
        return contactDto;
    }
}
