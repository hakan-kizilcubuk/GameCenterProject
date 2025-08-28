using GameCenterProject.Entities;
using GameCenterProject.ValueObjects;

namespace GameCenterProject.Application.Mapping;

public static class MappingExtensions
{
    // Money helpers
    public static string AsString(this Money m) => $"{m.Amount:0.00} {m.Currency}";
    public static (decimal amount, string currency) AsTuple(this Money m) => (m.Amount, m.Currency);

    // Quick summaries (returning anonymous objects via 'object' – handy in controllers if you don't want DTOs)
    public static object ToSummary(this Game g)
    => new {
        g.Id,
        g.Title,
        g.Description,
        Price = new { g.Price.Amount, g.Price.Currency },
        g.ReleaseDate,
        g.ImageUrl
    };

    public static object ToCartView(this Cart c)
        => new {
            c.UserId,
            Total = new { c.TotalPrice.Amount, c.TotalPrice.Currency },
            Items = c.Games.Select(g => new { g.Id, g.Title, Price = new { g.Price.Amount, g.Price.Currency } }).ToList()
        };

    public static object ToLibraryView(this Library l)
        => new {
            l.UserId,
            Games = l.OwnedGames.Select(g => new { g.Id, g.Title }).ToList()
        };
}
