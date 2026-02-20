using Microsoft.EntityFrameworkCore;
using SalesApi.Data;
using SalesApi.Dto;
using SalesApi.Middlewares;
using SalesApi.Services;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
 );

builder.Services.AddScoped<ProductServices>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddlewares>();

app.MapGet("/products", async (
    ProductServices service,
    string ? name,
    int page = 1,
    int pageSize = 10) =>
{
    var products = await service.GetFilteredAsync(name, page, pageSize);
    return Results.Ok(products);
});



app.MapGet("/product/{id}", async (int id, ProductServices service) =>
{
    var product = await service.GetProductByIdAsync(id);
    if (product is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(product);

});

app.MapPost("/products", async (CreateProductDto dto, ProductServices service) =>
{
    var product = await service.CreateProduct(dto);
    return Results.Created($"/products/{product.Id}", product);
});

app.MapPut("/products/{id}", async (int id, UpdateProductDto dto, ProductServices service) =>
{
    var product = await service.UpdateProductAsync(id, dto);
    return Results.Ok(product);
});

app.MapDelete("/products/{id}", async (int id, ProductServices service) =>
{
    var product = await service.DeleteProductAsync(id);
    return Results.Ok(product);
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
