using GameCenterProject.Application.Abstractions;
using GameCenterProject.Entities;
using GameCenterProject.Infrastructure.Abstract;


namespace GameCenterProject.Application.Services;
public sealed class LibraryService : ILibraryService
{
    private readonly ILibraryRepository _libraries;
    private readonly ICartRepository _carts;
    private readonly IGameRepository _games;
    private readonly IUnitOfWork _uow;

    public LibraryService(ILibraryRepository libraries, ICartRepository carts, IGameRepository games, IUnitOfWork uow)
    { _libraries = libraries; _carts = carts; _games = games; _uow = uow; }

    public async Task<Library> GetOrCreateAsync(string userId, CancellationToken ct = default)
    {
        var lib = await _libraries.FindAsync(userId, ct);
        if (lib is null)
        {
            lib = new Library(userId);
            await _libraries.AddAsync(lib, ct);
            await _uow.SaveChangesAsync(ct);
        }
        return lib;
    }

    public async Task<Library> AddGameAsync(string userId, Guid gameId, CancellationToken ct = default)
    {
        var lib = await GetOrCreateAsync(userId, ct);
        var game = await _games.FindAsync(gameId, ct) ?? throw new KeyNotFoundException("Game not found");
        lib.AddGame(game);
        await _uow.SaveChangesAsync(ct);
        return lib;
    }

    // Simple “checkout” that moves cart games into library and clears the cart.
    public async Task<Library> PurchaseCartAsync(string userId, CancellationToken ct = default)
    {
        var cart = await _carts.FindByUserAsync(userId, ct) ?? throw new InvalidOperationException("Cart not found");
        if (cart.Games.Count == 0) throw new InvalidOperationException("Cart is empty");

        var lib = await GetOrCreateAsync(userId, ct);
        foreach (var g in cart.Games.ToList())
        {
            lib.AddGame(g);
            cart.RemoveGame(g);  // keeps TotalPrice in sync
        }
        await _uow.SaveChangesAsync(ct);
        return lib;
    }
}