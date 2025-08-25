using GameCenterProject.Application;
using GameCenterProject.Application.Abstractions;
using GameCenterProject.Application.Services;
using GameCenterProject.Entities;
using GameCenterProject.Infrastructure;
using GameCenterProject.Infrastructure.Persistence;
using GameCenterProject.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

// ── Build host + DI
var builder = Host.CreateApplicationBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json", optional: true);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddApplication();                  // your services (Catalog/Cart/Library)
builder.Services.AddInfrastructure(builder.Configuration); // DbContext + EF repos + UoW

var app = builder.Build();

// ── Run a small demo
using var scope = app.Services.CreateScope();
var sp = scope.ServiceProvider;

var db = sp.GetRequiredService<AppDbContext>();
await db.Database.MigrateAsync();                   // apply migrations to MSSQL

await SeedIfEmpty(db);                              // add a couple of games if none

var catalog = sp.GetRequiredService<ICatalogService>();
var cartSvc  = sp.GetRequiredService<ICartService>();
var libSvc   = sp.GetRequiredService<ILibraryService>();

var userId = "alice";

// 1) List a few games
var games = await catalog.SearchAsync(q: null, page: 1, pageSize: 5);
Console.WriteLine("== Games ==");
foreach (var g in games)
    Console.WriteLine($"- {g.Title} | {g.Price.Amount:0.00} {g.Price.Currency} | year {g.ReleaseDate}");

// 2) Add first game to cart
var first = games.First();
await cartSvc.AddGameAsync(userId, first.Id);

var cart = await cartSvc.GetOrCreateAsync(userId);
Console.WriteLine($"\n== Cart for {userId} ==");
Console.WriteLine($"Items: {cart.Games.Count}, Total: {cart.TotalPrice.Amount:0.00} {cart.TotalPrice.Currency}");
foreach (var g in cart.Games)
    Console.WriteLine($"- {g.Title} ({g.Price.Amount:0.00} {g.Price.Currency})");

// 3) Purchase → moves games to library, clears cart
await libSvc.PurchaseCartAsync(userId);

var lib = await libSvc.GetOrCreateAsync(userId);
Console.WriteLine($"\n== Library for {userId} ==");
foreach (var g in lib.OwnedGames)
    Console.WriteLine($"- {g.Title}");

Console.WriteLine("\nDemo complete. Press any key to exit.");
Console.ReadKey();

// ── seed helper
static async Task SeedIfEmpty(AppDbContext db)
{
    if (!await db.Games.AnyAsync())
    {
        var g1 = new Game("Neon Odyssey",   "Fast-paced action in a neon city.", new Money(19.99m, "USD"), 2023);
        var g2 = new Game("Arcane Frontier","Explore a vast magical frontier.",  new Money(29.99m, "USD"), 2024);
        await db.Games.AddRangeAsync(g1, g2);
        await db.SaveChangesAsync();
    }
}

