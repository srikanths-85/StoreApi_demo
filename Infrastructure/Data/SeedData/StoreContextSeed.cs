using System.Text.Json;
using Core.Entities;

namespace Infrastructure.Data.SeedData;

public class StoreContextSeed
{
    public static async Task SeedAsync(StoreContext context)
    {
        if(!context.Products.Any())
        {
            var productData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/products.json");
            List<Product>? products = JsonSerializer.Deserialize<List<Product>>(productData);
            if(products == null)
                return ;
            await context.AddRangeAsync(products);
            await context.SaveChangesAsync();
        }
    }
}
