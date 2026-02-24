using Microsoft.AspNetCore.Mvc;
using SalesApi.Dto;
using SalesApi.Services;

[ApiController]
[Route("api/[controller]")]

public class SalesController : Controller
{
    private readonly ProductServices _productServices;
    public SalesController(ProductServices services)
    {
        _productServices = services;
    }

    [HttpGet]
    public async Task<ActionResult> GetProducts([FromQuery] string? name, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var products = await _productServices.GetFilteredAsync(name, page, pageSize);
        return Ok(products);
    }

    [HttpGet]
    public async Task<ActionResult> GetProductById([FromQuery] int id)
    {
        var product = await _productServices.GetProductByIdAsync(id);
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
         var product = await _productServices.CreateProduct(dto);
        return Ok(product);
    }

     [HttpPut]
     public async Task<ActionResult> UpdateProduct([FromBody] UpdateProductDto dto)
     {
         var product = await _productServices.UpdateProductAsync(dto.Id, dto);
         return Ok(product);
    }

}
