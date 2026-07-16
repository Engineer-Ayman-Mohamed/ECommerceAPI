using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using ECommerceAPI.Application.DTOs.Brand;
using ECommerceAPI.Application.Modules.Brands.Commands;
using ECommerceAPI.Application.Modules.Brands.Queries;

namespace ECommerceAPI.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class BrandsController : ControllerBase
{
    private readonly ISender _sender;

    public BrandsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    [AllowAnonymous]
    [ResponseCache(Duration = 300)]
    [ProducesResponseType(typeof(List<BrandToReturnDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBrands()
    {
        var result = await _sender.Send(new GetBrandsQuery());
        return Ok(result.Value);
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(BrandToReturnDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBrandById(int id)
    {
        var result = await _sender.Send(new GetBrandByIdQuery(id));
        if (!result.IsSuccess)
            return NotFound(new { message = result.Error, code = result.ErrorCode });
        return Ok(result.Value);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(BrandToReturnDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateBrand([FromBody] CreateBrandDto brand)
    {
        var result = await _sender.Send(new CreateBrandCommand(brand));
        return CreatedAtAction(nameof(GetBrandById), new { id = result.Value!.Id }, result.Value);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateBrand(int id, [FromBody] CreateBrandDto brand)
    {
        var result = await _sender.Send(new UpdateBrandCommand(id, brand));
        if (!result.IsSuccess)
            return NotFound(new { message = result.Error, code = result.ErrorCode });
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> DeleteBrand(int id)
    {
        var result = await _sender.Send(new DeleteBrandCommand(id));
        if (!result.IsSuccess)
        {
            if (result.ErrorCode == "BRAND_NOT_FOUND")
                return NotFound(new { message = result.Error, code = result.ErrorCode });
            return Conflict(new { message = result.Error, code = result.ErrorCode });
        }
        return NoContent();
    }
}
