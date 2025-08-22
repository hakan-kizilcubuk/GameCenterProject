using GameCenterProject.Application.Services;
using GameCenterProject.Application.Mapping;
using Microsoft.AspNetCore.Mvc;
using GameCenterProject.Application.Abstractions;

namespace GameCenterProject.Api.Controllers;

[ApiController]
[Route("api/users/{userId}/cart")]
public class CartController : ControllerBase
{
    private readonly ICartService _cart;
    public CartController(ICartService cart) => _cart = cart;

    [HttpGet]
    public async Task<IActionResult> Get(string userId, CancellationToken ct)
    {
        var cart = await _cart.GetOrCreateAsync(userId, ct);
        return Ok(cart.ToCartView());
    }

    public record AddItemRequest(Guid GameId);

    [HttpPost("items")]
    public async Task<IActionResult> Add(string userId, [FromBody] AddItemRequest body, CancellationToken ct)
    {
        var cart = await _cart.AddGameAsync(userId, body.GameId, ct);
        return Ok(cart.ToCartView());
    }

    [HttpDelete("items/{gameId:guid}")]
    public async Task<IActionResult> Remove(string userId, Guid gameId, CancellationToken ct)
    {
        var cart = await _cart.RemoveGameAsync(userId, gameId, ct);
        return Ok(cart.ToCartView());
    }

    [HttpDelete]
    public async Task<IActionResult> Clear(string userId, CancellationToken ct)
    {
        await _cart.ClearAsync(userId, ct);
        return NoContent();
    }
}
