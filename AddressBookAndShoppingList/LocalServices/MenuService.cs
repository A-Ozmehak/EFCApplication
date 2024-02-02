using Business.Dtos;
using Business.Interfaces;

namespace AddressBookAndShoppingList.LocalServices;

internal class MenuService(IContactService contactService, IProductService productService)
{
    private readonly IContactService _contactService = contactService;
    private readonly IProductService _productService = productService;


    public void Menu()
    {
        Console.Clear();
        Console.WriteLine("----- AddressBook & ShoppingList -----");

        while (true)
        {
            Console.WriteLine("\n");
            Console.WriteLine("What do you want to do?");

            Console.WriteLine("1.Handle Contacts");
            Console.WriteLine("2.Handle Shopping List");
            Console.WriteLine("3.Close application");

            var firstOption = Console.ReadLine();

            if (firstOption == "1")
            {
                Console.Clear();
                Console.WriteLine("ContactList");
                Console.WriteLine("-------------");
                Console.WriteLine("1.Add a new contact");
                Console.WriteLine("2.Get all contacts");
                Console.WriteLine("3.Get one contact");
                Console.WriteLine("4.Update a contact");
                Console.WriteLine("5.Delete a contact");
                Console.WriteLine("6.Close the application");
                Console.WriteLine();
                Console.WriteLine("Choose an option");

                var option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        Console.Clear();
                        AddContactOptions();
                        break;
                    case "2":
                        Console.Clear();
                        GetAllContactsOption();
                        break;
                    case "3":
                        Console.Clear();
                        GetOneContactOption();
                        break;
                    case "4":
                        Console.Clear();
                        UpdateContactOption();
                        break;
                    case "5":
                        Console.Clear();
                        DeleteContactOption();
                        break;
                    case "6":
                        CloseApplicationOption();
                        break;
                }
            }
            else if (firstOption == "2")
            { 
                Console.Clear();
                Console.WriteLine("ShoppingList");
                Console.WriteLine("-------------");
                Console.WriteLine("1.Add to shopping list");
                Console.WriteLine("2.See shopping list");
                Console.WriteLine("3.See one product");
                Console.WriteLine("4.Update a item in the shopping list");
                Console.WriteLine("5.Delete a item");
                Console.WriteLine("6.Close the application");
                Console.WriteLine();
                Console.WriteLine("Choose an option");

                var option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        Console.Clear();
                        AddProductOptions();
                        break;
                    case "2":
                        Console.Clear();
                        GetShoppingListOption();
                        break;
                    case "3":
                        Console.Clear();
                        GetOneProductOption();
                        break;
                    case "4":
                        Console.Clear();
                        UpdateProductOption();
                        break;
                    case "5":
                        Console.Clear();
                        DeleteProductOption();
                        break;
                    case "6":
                        CloseApplicationOption();
                        break;
                }
            } 
            else
            {
                CloseApplicationOption();
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
        var product = new ProductDto();

        Console.WriteLine("Add Product to shopping list");
        Console.WriteLine("-------------");

        Console.Write("Name: ");
        product.ProductName = Console.ReadLine()!;

        Console.Write("Price: ");
        product.Price = decimal.Parse(Console.ReadLine()!);

        Console.Write("Store: ");
        product.StoreName = Console.ReadLine()!;

        _productService.CreateProduct(product);
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

    public void GetShoppingListOption()
    {
        var products = _productService.GetAll() ?? new List<ProductDto>();

        Console.WriteLine("All products");
        Console.WriteLine("--------------");

        if (products == null)
        {
            Console.WriteLine("No products found");
            Console.WriteLine("\n");
        }
        else
        {
            foreach(var product in products)
            {
                Console.WriteLine($"Store: {product.StoreName}");
                Console.WriteLine($"Products in shopping list: {product.ProductName} {product.Price} kr");
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
            Console.Clear();
            Console.WriteLine($"{contact.FirstName} {contact.LastName}");
            Console.WriteLine($"{contact.Email} {contact.PhoneNumber}");
            Console.WriteLine($"{contact.StreetName} {contact.StreetNumber} {contact.PostalCode} {contact.City}");
            Console.WriteLine("\n\n");
        }
    }

    public void GetOneProductOption()
    {
        Console.WriteLine("Show product");
        Console.WriteLine("---------------");

        Console.WriteLine("Enter the Product name of the product you want to see: ");
        string productName = Console.ReadLine()!;
        ProductDto product = _productService.GetOne(productName);

        if (product == null)
        {
            Console.WriteLine("Can't find a product with that name");
        }
        else
        {
            Console.WriteLine($"Product and price: {product.ProductName} {product.Price} kr");
            Console.WriteLine($"Store: {product.StoreName}");
            Console.WriteLine("\n");
        }
    }

    public void UpdateContactOption()
    {
        Console.WriteLine("Update contact");
        Console.WriteLine("---------------");

        Console.Write("Enter the Email of the contact to update: ");
        string email = Console.ReadLine()!;

        ContactDto existingContact = _contactService.GetOne(email);
        if (existingContact == null)
        {
            Console.WriteLine("\n");
            Console.WriteLine("A contact with this email does not exist");
            return;
        }

        Console.Write("Enter a new first name: ");
        string firstName = Console.ReadLine()!;

        Console.Write("Enter a new last name: ");
        string lastName = Console.ReadLine()!;

        Console.Write("Enter a new email: ");
        string emailAddress = Console.ReadLine()!;

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
            Email = emailAddress,
            PhoneNumber = phoneNumber,
            StreetName = streetName,
            StreetNumber = streetNumber,
            PostalCode = postalCode,
            City = city
        };

        _contactService.Update(updatedContactDto);
    }

    public void UpdateProductOption()
    {
        Console.WriteLine("Update a product");
        Console.WriteLine("---------------");

        Console.Write("Enter the ProductName of the product to update: ");
        string product = Console.ReadLine()!;

        ProductDto existingProduct = _productService.GetOne(product);
        if (existingProduct == null)
        {
            Console.WriteLine("\n");
            Console.WriteLine("There is no product with that name on the list!");
            return;
        }

        Console.Clear();
        Console.Write("Enter a new product name: ");
        string productName = Console.ReadLine()!;

        Console.Write("Enter a new price: ");
        decimal price = decimal.Parse(Console.ReadLine()!);

        Console.Write("Enter a new store name: ");
        string storeName = Console.ReadLine()!;

        var updatedProductDto = new ProductDto
        {
            ProductName = productName,
            Price = price,
            StoreName = storeName
        };

        _productService.Update(updatedProductDto);
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

    public void DeleteProductOption()
    {
        Console.WriteLine("Remove Product");
        Console.WriteLine("---------------");

        Console.WriteLine("Enter the ProductName of the product you want to remove: ");
        string productName = Console.ReadLine()!;

        if (productName == null)
        {
            Console.WriteLine("Can't find a product with that name");
        }
        else
        {
            Console.WriteLine("The product is now removed");
            _productService.Remove(productName);

        }
    }

    public void CloseApplicationOption()
    {
        Environment.Exit(0);
    }
}
