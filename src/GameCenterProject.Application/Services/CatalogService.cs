using GameCenterProject.Application.Abstractions;
using GameCenterProject.Entities;
using GameCenterProject.Infrastructure.Abstract;

using GameCenterProject.ValueObjects;


namespace GameCenterProject.Application.Services;

public sealed class CatalogService : ICatalogService
{
    private readonly IGameRepository _games;
    private readonly IDiscountRepository _discounts;
    public CatalogService(IGameRepository games, IDiscountRepository discounts)
    {
        _games = games;
        _discounts = discounts;
    }

    public async Task<IReadOnlyList<Game>> SearchAsync(string? q, int page = 1, int pageSize = 20, CancellationToken ct = default)
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 100);
        var skip = (page - 1) * pageSize;
        var games = await _games.SearchAsync(q, skip, pageSize, ct);
        var now = DateTime.UtcNow;
        var result = new List<Game>();
        foreach (var game in games)
        {
            var discounts = await _discounts.ListByGameAsync(game.Id, ct);
            var active = discounts.FirstOrDefault(d => d.StartDate <= now && d.EndDate >= now);
            if (active != null)
            {
                game.ChangePrice(CalculateDiscountedPrice(game.Price, active));
            }
            result.Add(game);
        }
        return result;
    }

    public async Task<Game?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var game = await _games.FindAsync(id, ct);
        if (game == null) return null;
        var now = DateTime.UtcNow;
        var discounts = await _discounts.ListByGameAsync(game.Id, ct);
        var active = discounts.FirstOrDefault(d => d.StartDate <= now && d.EndDate >= now);
        if (active != null)
        {
            game.ChangePrice(CalculateDiscountedPrice(game.Price, active));
        }
        return game;
    }

    private Money CalculateDiscountedPrice(Money original, Discount discount)
    {
        if (discount.Amount.HasValue)
        {
            var newAmount = Math.Max(0, original.Amount - discount.Amount.Value);
            return new Money(newAmount, original.Currency);
        }
        else
        {
            var newAmount = Math.Max(0, original.Amount * (1 - discount.Percentage));
            return new Money(newAmount, original.Currency);
        }
    }
}