using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesApi.Dto.Product;
using SalesApi.Services;
using System.Security.Claims;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProductController : Controller
{
    private readonly ProductServices _productServices;
    public ProductController(ProductServices services)
    {
        _productServices = services;
    }


    [HttpGet]
    public async Task<ActionResult> GetProducts([FromQuery] string? name, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var products = await _productServices.GetFilteredAsync(name, page, pageSize, userId);
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetProductById(int id)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var product = await _productServices.GetProductByIdAsync(id, userId);
        return Ok(product);
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteProduct([FromQuery] int id)
    {
        
        var product = await _productServices.DeleteProductAsync(id);
        return Ok(product);
     }

    [HttpPost]
    public async Task<ActionResult> CreateProduct([FromBody] CreateProductDto dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var product = await _productServices.CreateProduct(dto, userId);

        return Ok(product);
    }

    [HttpPut]
     public async Task<ActionResult> UpdateProduct([FromBody] UpdateProductDto dto)
     {
         var product = await _productServices.UpdateProductAsync(dto.Id, dto);
         return Ok(product);
    }

}
