using GameCenterProject.Application.Abstractions;
using GameCenterProject.Entities;
using GameCenterProject.Infrastructure.Abstract;


namespace GameCenterProject.Application.Services;
public sealed class CartService : ICartService
{
    private readonly ICartRepository _carts;
    private readonly IGameRepository _games;
    private readonly IUnitOfWork _uow;

    public CartService(ICartRepository carts, IGameRepository games, IUnitOfWork uow)
    { _carts = carts; _games = games; _uow = uow; }

    public async Task<Cart> GetOrCreateAsync(string userId, CancellationToken ct = default)
    {
        var cart = await _carts.FindByUserAsync(userId, ct);
        if (cart is null)
        {
            cart = new Cart(userId);
            await _carts.AddAsync(cart, ct);
            await _uow.SaveChangesAsync(ct);
        }
        return cart;
    }

    public async Task<Cart> AddGameAsync(string userId, Guid gameId, CancellationToken ct = default)
    {
        var cart = await GetOrCreateAsync(userId, ct);
        var game = await _games.FindAsync(gameId, ct) ?? throw new KeyNotFoundException("Game not found");
        cart.AddGame(game);              // updates TotalPrice & UpdatedAt in your entity
        await _uow.SaveChangesAsync(ct);
        return cart;
    }

    public async Task<Cart> RemoveGameAsync(string userId, Guid gameId, CancellationToken ct = default)
    {
        var cart = await _carts.FindByUserAsync(userId, ct) ?? throw new KeyNotFoundException("Cart not found");
        var game = await _games.FindAsync(gameId, ct) ?? throw new KeyNotFoundException("Game not found");
        cart.RemoveGame(game);           // updates TotalPrice & UpdatedAt in your entity
        await _uow.SaveChangesAsync(ct);
        return cart;
    }

    public async Task ClearAsync(string userId, CancellationToken ct = default)
    {
        var cart = await _carts.FindByUserAsync(userId, ct);
        if (cart is null) return;

        // remove all games one by one to keep TotalPrice consistent
        foreach (var g in cart.Games.ToList())
            cart.RemoveGame(g);

        await _uow.SaveChangesAsync(ct);
    }
}