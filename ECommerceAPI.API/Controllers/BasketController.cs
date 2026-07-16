using MediatR;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using ECommerceAPI.Application.DTOs.Basket;
using ECommerceAPI.Application.Modules.Basket.Commands;
using ECommerceAPI.Application.Modules.Basket.Queries;

namespace ECommerceAPI.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class BasketController : ControllerBase
{
    private readonly ISender _sender;

    public BasketController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CustomerBasketDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBasket(string id)
    {
        var result = await _sender.Send(new GetBasketQuery(id));
        if (!result.IsSuccess)
            return NotFound(new { message = result.Error, code = result.ErrorCode });
        return Ok(result.Value);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CustomerBasketDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateBasket([FromBody] CustomerBasketDto basket)
    {
        var result = await _sender.Send(new UpdateBasketCommand(basket));
        return Ok(result.Value);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteBasket(string id)
    {
        var result = await _sender.Send(new DeleteBasketCommand(id));
        if (!result.IsSuccess)
            return BadRequest(new { message = result.Error, code = result.ErrorCode });
        return NoContent();
    }
}
