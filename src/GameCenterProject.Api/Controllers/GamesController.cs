using GameCenterProject.Application.Services;
using GameCenterProject.Application.Mapping;
using Microsoft.AspNetCore.Mvc;

namespace GameCenterProject.Api.Controllers;

[ApiController]
[Route("api/games")]
public class GamesController : ControllerBase
{
    private readonly ICatalogService _catalog;
    public GamesController(ICatalogService catalog) => _catalog = catalog;

    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] string? q, [FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
    {
        var games = await _catalog.SearchAsync(q, page, pageSize, ct);
        // Return simple projections (no DTO types)
        var data = games.Select(g => g.ToSummary());
        return Ok(data);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var game = await _catalog.GetByIdAsync(id, ct);
        if (game is null) return NotFound();
        return Ok(game.ToSummary());
    }
}
