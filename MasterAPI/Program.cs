using MasterAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.SuppressModelStateInvalidFilter = true;

});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ShopContext>(
    options =>
    {
        options.UseInMemoryDatabase("Shop");
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetService<ShopContext>();
    await db.Database.EnsureCreatedAsync();
}

app.MapGet("/products", async (ShopContext _context) =>
{
    return await _context.Products.ToListAsync();
});

app.MapGet("/products/{id}", async (int id, ShopContext _context) =>
{
    var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

    return product != null ? Results.Ok(product) : Results.NotFound() ;
});

app.MapGet("/products/available", async (ShopContext _context) =>
{
    var productAvailable = await _context.Products.Where(x => x.IsAvailable).ToListAsync();

    return productAvailable != null ? Results.Ok(productAvailable) : Results.NotFound();
});

app.Run();
