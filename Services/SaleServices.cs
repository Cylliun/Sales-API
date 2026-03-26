using Microsoft.EntityFrameworkCore;
using SalesApi.Data;
using SalesApi.Dto.Sales;
using SalesApi.Dto.SalesItem;
using SalesApi.Models;
using System.Net.ServerSentEvents;

namespace SalesApi.Services;

public class SaleServices
{
    private readonly DataContext _context;
    public SaleServices(DataContext context)
    {
        _context = context;
    }

    public async Task<Sale> CreateSale(CreateSalesDto dto, int userId)
    {
        if (dto.Items == null || !dto.Items.Any())
            throw new ArgumentException("Sale must contain items.");

        var productIds = dto.Items
            .Select(i => i.ProductId)
            .Distinct()
            .ToList();

        var products = await _context.Products
            .Where(p => productIds.Contains(p.Id) && p.UserId == userId)
            .ToListAsync();

        if (products.Count != productIds.Count)
            throw new Exception("Invalid products.");

        var sale = new Sale(userId);

        foreach (var item in dto.Items)
        {
            var product = products.First(p => p.Id == item.ProductId);

            if (product.Stock < item.Quantity)
                throw new Exception($"Insufficient stock for product {product.Name}");

            var saleItem = new SalesItem(
                product.Id,
                item.Quantity,
                product.Price
            );

            sale.AddItem(saleItem);

            product.ReduceStock(item.Quantity);
        }

        _context.Sales.Add(sale);

        await _context.SaveChangesAsync();

        return sale;
    }

    public async Task<SalesDto> GetSalesByIdAsync(int id, int userId)
    {
        var sale = await _context.Sales
            .Where(s => s.Id == id && s.UserId == userId)
            .Include(s => s.Items)
            .ThenInclude(i => i.Product)
            .Select(s => new SalesDto
            {
                Id = s.Id,
                TotalAmount = s.TotalAmount,
                CreatedAt = s.CreatedAt,
                Items = s.Items.Select(i => new SaleItemDto
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    SubTotal = i.CalculateSubTotal()
                }).ToList()
            })
        .FirstOrDefaultAsync();

        if (sale == null)
            throw new KeyNotFoundException("Sales not found");
        

        return sale;

    }

    public async Task<List<SalesDto>> GetSales(int userId)
    {
        return await _context.Sales
            .Where(s => s.UserId == userId)
            .Include(s => s.Items)
            .ThenInclude(i => i.Product)
            .Select(s => new SalesDto
            {
                Id = s.Id,
                TotalAmount = s.TotalAmount,
                CreatedAt = s.CreatedAt,
                Items = s.Items.Select(i => new SaleItemDto
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    SubTotal = i.CalculateSubTotal()
                }).ToList()
            })
        .ToListAsync();

    }

    public async Task<Sale> CancelSale(int id, int userId)
    {
        var sale = await _context.Sales
            .Include(s => s.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId);

        if (sale == null)
            throw new KeyNotFoundException("Sale not found.");

        if (sale.IsCanceled)
            throw new InvalidOperationException("Sale is already canceled.");

        sale.Cancel();

        foreach (var item in sale.Items)
        {
            if (item.Product == null)
                throw new Exception("Product not found.");

            item.Product.IncreaseStock(item.Quantity);
        }

        await _context.SaveChangesAsync();

        return sale;
    }

}
