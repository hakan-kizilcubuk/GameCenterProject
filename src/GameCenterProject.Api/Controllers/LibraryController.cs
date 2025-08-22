using GameCenterProject.Application.Services;
using GameCenterProject.Application.Mapping;
using Microsoft.AspNetCore.Mvc;

namespace GameCenterProject.Api.Controllers;

[ApiController]
[Route("api/users/{userId}/library")]
public class LibraryController : ControllerBase
{
    private readonly ILibraryService _library;
    public LibraryController(ILibraryService library) => _library = library;

    [HttpGet]
    public async Task<IActionResult> Get(string userId, CancellationToken ct)
    {
        var lib = await _library.GetOrCreateAsync(userId, ct);
        return Ok(lib.ToLibraryView());
    }

    public record AddLibraryItemRequest(Guid GameId);

    [HttpPost("items")]
    public async Task<IActionResult> Add(string userId, [FromBody] AddLibraryItemRequest body, CancellationToken ct)
    {
        var lib = await _library.AddGameAsync(userId, body.GameId, ct);
        return Ok(lib.ToLibraryView());
    }

    [HttpPost("purchase")]
    public async Task<IActionResult> Purchase(string userId, CancellationToken ct)
    {
        var lib = await _library.PurchaseCartAsync(userId, ct);
        return Ok(lib.ToLibraryView());
    }
}
