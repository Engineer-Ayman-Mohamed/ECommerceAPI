using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using ECommerceAPI.Domain.Entities;

namespace ECommerceAPI.Infrastructure.Data.Seed;

public static class SeedData
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (await context.ProductBrands.AnyAsync()) return;

        var brands = await ReadJsonAsync<List<ProductBrand>>("brands.json");
        var types = await ReadJsonAsync<List<ProductType>>("types.json");
        var products = await ReadJsonAsync<List<Product>>("products.json");
        var deliveryMethods = await ReadJsonAsync<List<DeliveryMethod>>("delivery.json");

        if (brands is not null)
            await context.ProductBrands.AddRangeAsync(brands);

        if (types is not null)
            await context.ProductTypes.AddRangeAsync(types);

        if (deliveryMethods is not null)
            await context.DeliveryMethods.AddRangeAsync(deliveryMethods);

        await context.SaveChangesAsync();

        if (products is not null)
        {
            var dbBrands = await context.ProductBrands.ToListAsync();
            var dbTypes = await context.ProductTypes.ToListAsync();

            foreach (var product in products)
            {
                product.ProductBrandId = dbBrands.First(b => b.Id == product.ProductBrandId).Id;
                product.ProductTypeId = dbTypes.First(t => t.Id == product.ProductTypeId).Id;
            }

            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();
        }
    }

    private static async Task<T?> ReadJsonAsync<T>(string fileName)
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Data", "Seed", fileName);

        if (!File.Exists(path))
            return default;

        var json = await File.ReadAllTextAsync(path);
        return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
}
