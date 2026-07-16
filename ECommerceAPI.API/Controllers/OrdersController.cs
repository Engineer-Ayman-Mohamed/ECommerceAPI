using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using ECommerceAPI.Application.DTOs.Order;
using ECommerceAPI.Application.DTOs.DeliveryMethod;
using ECommerceAPI.Application.Modules.Orders.Commands;
using ECommerceAPI.Application.Modules.Orders.Queries;
using ECommerceAPI.Domain.Enums;

namespace ECommerceAPI.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class OrdersController : ControllerBase
{
    private readonly ISender _sender;

    public OrdersController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("{id:int}")]
    [Authorize]
    [ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOrderById(int id)
    {
        var result = await _sender.Send(new GetOrderByIdQuery(id));
        if (!result.IsSuccess)
            return NotFound(new { message = result.Error, code = result.ErrorCode });
        return Ok(result.Value);
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(List<OrderToReturnDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOrdersForUser()
    {
        var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
        var result = await _sender.Send(new GetOrdersForUserQuery(email!));
        return Ok(result.Value);
    }

    [HttpGet("delivery-methods")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(List<DeliveryMethodToReturnDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDeliveryMethods()
    {
        var result = await _sender.Send(new GetDeliveryMethodsQuery());
        return Ok(result.Value);
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto order)
    {
        var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
        order.BuyerEmail = email;

        var result = await _sender.Send(new CreateOrderCommand(order));
        if (!result.IsSuccess)
            return BadRequest(new { message = result.Error, code = result.ErrorCode });
        return CreatedAtAction(nameof(GetOrderById), new { id = result.Value!.Id }, result.Value);
    }

    [HttpPut("{id:int}/status")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] OrderStatus status)
    {
        var result = await _sender.Send(new UpdateOrderStatusCommand(id, status));
        if (!result.IsSuccess)
            return NotFound(new { message = result.Error, code = result.ErrorCode });
        return Ok(result.Value);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var result = await _sender.Send(new DeleteOrderCommand(id));
        if (!result.IsSuccess)
            return NotFound(new { message = result.Error, code = result.ErrorCode });
        return NoContent();
    }
}
