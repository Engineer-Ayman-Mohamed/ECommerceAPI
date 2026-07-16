using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using ECommerceAPI.Application.DTOs.Product;
using ECommerceAPI.Application.Modules.Products.Commands;
using ECommerceAPI.Application.Modules.Products.Queries;
using ECommerceAPI.Application.Shared;

namespace ECommerceAPI.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class ProductsController : ControllerBase
{
    private readonly ISender _sender;

    public ProductsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    [AllowAnonymous]
    [ResponseCache(Duration = 300)]
    [ProducesResponseType(typeof(Pagination<ProductToReturnDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProducts(
        [FromQuery] int? brandId,
        [FromQuery] int? typeId,
        [FromQuery] string? sort,
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _sender.Send(new GetProductsQuery(brandId, typeId, sort, pageIndex, pageSize));
        return Ok(result.Value);
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProductById(int id)
    {
        var result = await _sender.Send(new GetProductByIdQuery(id));
        if (!result.IsSuccess)
            return NotFound(new { message = result.Error, code = result.ErrorCode });
        return Ok(result.Value);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto product)
    {
        var result = await _sender.Send(new CreateProductCommand(product));
        Response.Headers["Cache-Control"] = "no-cache";
        return CreatedAtAction(nameof(GetProductById), new { id = result.Value!.Id }, result.Value);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto product)
    {
        var result = await _sender.Send(new UpdateProductCommand(id, product));
        if (!result.IsSuccess)
            return NotFound(new { message = result.Error, code = result.ErrorCode });
        Response.Headers["Cache-Control"] = "no-cache";
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var result = await _sender.Send(new DeleteProductCommand(id));
        if (!result.IsSuccess)
            return NotFound(new { message = result.Error, code = result.ErrorCode });
        Response.Headers["Cache-Control"] = "no-cache";
        return NoContent();
    }

    [HttpPost("{id:int}/image")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [RequestSizeLimit(5 * 1024 * 1024)]
    public async Task<IActionResult> UploadProductImage(int id, IFormFile file)
    {
        if (file is null || file.Length == 0)
            return BadRequest(new { message = "No file uploaded" });

        using var stream = file.OpenReadStream();
        var result = await _sender.Send(new UploadProductImageCommand(id, stream, file.FileName));

        if (!result.IsSuccess)
            return NotFound(new { message = result.Error, code = result.ErrorCode });

        return Ok(result.Value);
    }
}
