using AddressBookAndShoppingList.LocalServices;
using Business.Interfaces;
using Business.Services;
using Infrastructure.Contexts;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder().ConfigureServices(services =>
{
    services.AddDbContext<ContactContext>(x => x.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Anna\Documents\Repos\EFCApplication\Infrastructure\Data\Contact_database.mdf;Integrated Security=True;Connect Timeout=30"));

    services.AddSingleton<IContactRepository, ContactRepository>();
    services.AddSingleton<IContactService, ContactService>();

    services.AddSingleton<MenuService>();

}).Build();

builder.Start();

var menuService = builder.Services.GetRequiredService<MenuService>();
menuService.Menu();


