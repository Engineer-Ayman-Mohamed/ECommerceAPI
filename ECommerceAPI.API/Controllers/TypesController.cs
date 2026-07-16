using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using ECommerceAPI.Application.DTOs.Type;
using ECommerceAPI.Application.Modules.Types.Commands;
using ECommerceAPI.Application.Modules.Types.Queries;

namespace ECommerceAPI.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class TypesController : ControllerBase
{
    private readonly ISender _sender;

    public TypesController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    [AllowAnonymous]
    [ResponseCache(Duration = 300)]
    [ProducesResponseType(typeof(List<TypeToReturnDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTypes()
    {
        var result = await _sender.Send(new GetTypesQuery());
        return Ok(result.Value);
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(TypeToReturnDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTypeById(int id)
    {
        var result = await _sender.Send(new GetTypeByIdQuery(id));
        if (!result.IsSuccess)
            return NotFound(new { message = result.Error, code = result.ErrorCode });
        return Ok(result.Value);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(TypeToReturnDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateType([FromBody] CreateTypeDto type)
    {
        var result = await _sender.Send(new CreateTypeCommand(type));
        return CreatedAtAction(nameof(GetTypeById), new { id = result.Value!.Id }, result.Value);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateType(int id, [FromBody] CreateTypeDto type)
    {
        var result = await _sender.Send(new UpdateTypeCommand(id, type));
        if (!result.IsSuccess)
            return NotFound(new { message = result.Error, code = result.ErrorCode });
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> DeleteType(int id)
    {
        var result = await _sender.Send(new DeleteTypeCommand(id));
        if (!result.IsSuccess)
        {
            if (result.ErrorCode == "TYPE_NOT_FOUND")
                return NotFound(new { message = result.Error, code = result.ErrorCode });
            return Conflict(new { message = result.Error, code = result.ErrorCode });
        }
        return NoContent();
    }
}
