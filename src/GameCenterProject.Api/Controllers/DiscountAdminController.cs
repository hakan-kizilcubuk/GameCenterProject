using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GameCenterProject.Application.Abstractions;
using GameCenterProject.Entities;

namespace GameCenterProject.Api.Controllers;

[ApiController]
[Route("api/admin/discounts")]
public class DiscountAdminController : ControllerBase
{
    private readonly IDiscountService _discounts;
    public DiscountAdminController(IDiscountService discounts) => _discounts = discounts;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Discount discount, [FromQuery] string adminUserId, CancellationToken ct)
    {
    // Ignore CreatedBy from client, set it in service
    discount.CreatedBy = "";
        var d = await _discounts.CreateAsync(discount, adminUserId, ct);
        return Ok(d);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Remove(Guid id, [FromQuery] string adminUserId, CancellationToken ct)
    {
        await _discounts.RemoveAsync(id, adminUserId, ct);
        return NoContent();
    }
}
