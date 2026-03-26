using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesApi.Dto.Sales;
using SalesApi.Services;
using System.Security.Claims;

namespace SalesApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SalesController : ControllerBase
{
    private readonly SaleServices _saleServices;

    public SalesController(SaleServices services)
    {
        _saleServices = services;
    }

    [HttpPost]
    public async Task<IActionResult> CreateSale([FromBody] CreateSalesDto dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var sale = await _saleServices.CreateSale(dto, userId);
        return Ok(sale);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSaleById(int id)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var sale = await _saleServices.GetSalesByIdAsync(id, userId);
        return Ok(sale);
    }
    [HttpGet]
    public async Task<IActionResult> GetAllSales()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var sales = await _saleServices.GetSales(userId);
        return Ok(sales);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSale(int id)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var result = await _saleServices.CancelSale(id, userId);
        return Ok(result);
    }
}
