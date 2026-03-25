using Microsoft.EntityFrameworkCore;
using SalesApi.Data;
using SalesApi.Dto.Product;
using SalesApi.Models;

namespace SalesApi.Services;

public class ProductServices
{
    private readonly DataContext _context;

    public ProductServices(DataContext context) 
    {
        _context = context;
    }

    public async Task<Product> CreateProduct(CreateProductDto dto, int userId)
    {
        var product = new Product(dto.Name, dto.Price, dto.Stock, userId);

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return product;
    }

    public async Task<List<Product>> GetFilteredAsync(string? name, int page, int pageSize, int userId)
    {

        var query = _context.Products
            .Where(p => p.UserId ==userId);

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(p => p.Name.Contains(name));

        query = query
            .Skip((page - 1) * pageSize)
            .Take(pageSize);

        return await query.ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id, int userId)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == id && p.UserId ==userId);
        return product;
    }

    public async Task<Product> UpdateProductAsync(int id, UpdateProductDto dto)
    {
        var product = await _context.Products.FindAsync(dto.Id);

        if (product is null) 
            throw new KeyNotFoundException("Product not found");

        product.Update(dto.Name, dto.Price);

        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<Product> DeleteProductAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product is null) 
            throw new KeyNotFoundException("Product not found");

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return product;
    }

}
