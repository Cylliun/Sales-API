using Microsoft.EntityFrameworkCore;
using SalesApi.Data;
using SalesApi.Dto.Sales;
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
            throw new Exception("One or more products do not exist.");

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

    public async Task<Sale?> GetSalesByIdAsync(int id, int userId)
    {
        var sale = await _context.Sales
            .Include(s => s.Items)
            .ThenInclude(i => i.ProductId)
            .FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId);

        if (sale == null)
            throw new KeyNotFoundException("Sale not found.");

        return sale;
    }

    public async Task<List<Sale>> GetSales(int userId)
    {
        var sale = await _context.Sales
            .Where(s => s.UserId == userId)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();

        if (sale.Count == 0)
            throw new KeyNotFoundException("Sale not found");

        return sale;
    } 

    public async Task<Sale> CancelSale(int id, int userId)
    {
        var sale = await _context.Sales
            .Include(s => s.Items)
            .FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId);
        if (sale == null)
            throw new KeyNotFoundException("Sale not found.");
        if (sale.IsCanceled)
            throw new InvalidOperationException("Sale is already canceled.");
        sale.Cancel();
        foreach (var item in sale.Items)
        {
            var product = await _context.Products.FindAsync(item.ProductId);
            if (product != null)
            {
                product.IncreaseStock(item.Quantity);
            }
        }
        await _context.SaveChangesAsync();
        return sale;
    }

}
