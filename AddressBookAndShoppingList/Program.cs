using AddressBookAndShoppingList.LocalServices;
using Business.Interfaces;
using Business.Services;
using Infrastructure.Contexts;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateDefaultBuilder().ConfigureServices(services =>
{
    services.AddDbContext<ContactContext>(x => x.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Anna\Documents\Repos\EFCApplication\Infrastructure\Data\Contact_database.mdf;Integrated Security=True;Connect Timeout=30"));
    services.AddDbContext<ProductCatalogContext>(x => x.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Anna\Documents\Repos\EFCApplication\Infrastructure\Data\ProductCatalog.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=True"));

    services.AddSingleton<IContactRepository, ContactRepository>();
    services.AddSingleton<IContactService, ContactService>();

    services.AddSingleton<IAddressRepository, AddressRepository>();
    services.AddSingleton<IPhoneNumberRepository, PhoneNumberRepository>();

    services.AddSingleton<IProductRepository, ProductRepository>();
    services.AddSingleton<IProductService, ProductService>();

    services.AddSingleton<IStoreRepository, StoreRepository>();

    services.AddSingleton<MenuService>();

}).ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
    logging.AddFilter("Microsoft.EntityFrameworkCore", LogLevel.Warning);
})
    .Build();



builder.Start();

var menuService = builder.Services.GetRequiredService<MenuService>();
menuService.Menu();


