using Business.Dtos;
using Business.Interfaces;

namespace AddressBookAndShoppingList.LocalServices;

internal class MenuService(IContactService contactService)
{
    private readonly IContactService _contactService = contactService;


    public void Menu()
    {
        Console.Clear();
        Console.WriteLine("----- AddressBook & ShoppingList -----");

        while (true)
        {
            Console.WriteLine("What do you want to do?");
            Console.WriteLine("1.Add a new contact");
            Console.WriteLine("2.Add a product to the shopping list");
            Console.WriteLine("3.Get all contacts");
            Console.WriteLine("4.Get one contact");
            Console.WriteLine("5.Update a contact");
            Console.WriteLine("6.Delete a contact");
            Console.WriteLine("7.Close the application");
            Console.WriteLine();
            Console.WriteLine("Choose an option");

            var option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    AddContactOptions();
                    break;
                case "2":
                    AddProductOptions();
                    break;
                case "3":
                    GetAllContactsOption();
                    break;
                case "4":
                    GetOneContactOption();
                    break;
                case "5":
                    UpdateContactOption();
                    break;
                case "6":
                    DeleteContactOption();
                    break;
                case "7":
                    CloseApplicationOption();
                    break;
            }
        }
    }

    public void AddContactOptions()
    {
        var contact = new ContactDto();

        Console.WriteLine("Add Contact");
        Console.WriteLine("-------------");

        Console.Write("FirstName: ");
        contact.FirstName = Console.ReadLine()!;

        Console.Write("LastName: ");
        contact.LastName = Console.ReadLine()!;

        Console.Write("Email: ");
        contact.Email = Console.ReadLine()!;

        Console.Write("PhoneNumber: ");
        contact.PhoneNumber = Console.ReadLine()!;

        Console.Write("StreetName: ");
        contact.StreetName = Console.ReadLine()!;

        Console.Write("StreetNumber: ");
        contact.StreetNumber = Console.ReadLine()!;

        Console.Write("PostalCode: ");
        contact.PostalCode = Console.ReadLine()!;

        Console.Write("City: ");
        contact.City = Console.ReadLine()!;

        _contactService.CreateContact(contact);
    }

    public void AddProductOptions()
    {
        //var product = new ProductDto();

        //Console.WriteLine("Add Product to shopping list");
        //Console.WriteLine("-------------");

        //Console.Write("Name: ");
        //product.Title = Console.ReadLine()!;

        //Console.Write("Price: ");
        //product.Price = decimal.Parse(Console.ReadLine()!);

        //Console.Write("Store: ");
        //product.StoreName = Console.ReadLine()!;
    }

    public void GetAllContactsOption()
    {
        var contacts = _contactService.GetAll() ?? new List<ContactDto>();

        Console.WriteLine("All contacts");
        Console.WriteLine("--------------");

        if (contacts == null)
        { 
            Console.WriteLine("No contacts found");
            Console.WriteLine("\n");
        }
        else
        {
            foreach (var contact in contacts)
            {
                Console.WriteLine($"Name: {contact.FirstName} {contact.LastName}");
                Console.WriteLine($"Email: {contact.Email}");
                Console.WriteLine($"Phone number: {contact.PhoneNumber}");
                Console.WriteLine($"Address: {contact.StreetName} {contact.StreetNumber} {contact.PostalCode} {contact.City}");
                Console.WriteLine("\n");
            }
        }
    }

    public void GetOneContactOption()
    {
        Console.WriteLine("Show Contact");
        Console.WriteLine("---------------");

        Console.WriteLine("Enter the email of the contact you want to see: ");
        string email = Console.ReadLine()!;
        ContactDto contact = _contactService.GetOne(email);

        if (contact == null)
        {
            Console.WriteLine("Can't find a contact with that email");
        }
        else
        {
            Console.WriteLine($"{contact.FirstName} {contact.LastName}");
            Console.WriteLine($"{contact.Email} {contact.PhoneNumber}");
            Console.WriteLine($"{contact.StreetName} {contact.StreetNumber} {contact.PostalCode} {contact.City}");
            Console.WriteLine("\n\n");
        }
    }

    public void UpdateContactOption()
    {
        Console.WriteLine("Update contact");
        Console.WriteLine("---------------");

        Console.Write("Enter the Email of the contact to update: ");
        string existingEmail = Console.ReadLine()!;

        Console.Write("Enter a new first name: ");
        string firstName = Console.ReadLine()!;

        Console.Write("Enter a new last name: ");
        string lastName = Console.ReadLine()!;

        Console.Write("Enter a new email: ");
        string email = Console.ReadLine()!;

        Console.Write("Enter a new phone number: ");
        string phoneNumber = Console.ReadLine()!;

        Console.Write("Enter a new street name: ");
        string streetName = Console.ReadLine()!;

        Console.Write("Enter a new street number: ");
        string streetNumber = Console.ReadLine()!;

        Console.Write("Enter a new postal code: ");
        string postalCode = Console.ReadLine()!;

        Console.Write("Enter a new city: ");
        string city = Console.ReadLine()!;

        var updatedContactDto = new ContactDto
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            PhoneNumber = phoneNumber,
            StreetName = streetName,
            StreetNumber = streetNumber,
            PostalCode = postalCode,
            City = city
        };

        _contactService.Update(updatedContactDto);
    }

    public void DeleteContactOption()
    {
        Console.WriteLine("Remove Contact");
        Console.WriteLine("---------------");

        Console.WriteLine("Enter the email of the contact you want to remove: ");
        string email = Console.ReadLine()!;

        if (email == null)
        {
            Console.WriteLine("Can't find a contact with that email");
        }
        else
        {
            Console.WriteLine("The contact is now removed");
            _contactService.Remove(email);

        }
    }

    public void CloseApplicationOption()
    {
        Environment.Exit(0);
    }
}
