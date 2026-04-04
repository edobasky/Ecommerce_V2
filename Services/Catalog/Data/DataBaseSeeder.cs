using System;
using System.Text.Json;
using Catalog.Entities;
using MongoDB.Driver;

namespace Catalog.Data
{
    public class DataBaseSeeder
    {
        public static async Task SeedAsync(IConfiguration config)
        {
            var client = new MongoClient(config["DatabaseSettings:ConnectionString"]);
            var db = client.GetDatabase(config["DatabaseSettings:DatabaseName"]);
            var products = db.GetCollection<Product>(config["DatabaseSettings:ProductCollectionName"]);
            var brands = db.GetCollection<ProductBrand>(config["DatabaseSettings:BrandCollectionName"]);
            var types = db.GetCollection<ProductType>(config["DatabaseSettings:TypeCollectionName"]);

            var SeedBasePath = Path.Combine("Data","SeedData");

            // Seeb Brands
            List<ProductBrand> brandList = new();
            if ((await brands.CountDocumentsAsync(_=> true)) == 0)
            {
                var brandData = await File.ReadAllTextAsync(Path.Combine(SeedBasePath, "brands.json"));
                brandList = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);
                await brands.InsertManyAsync(brandList);
            }else
            {
                brandList = await brands.Find(_ => true).ToListAsync();
            }

            // Seeb Types
            List<ProductType> typeList = new();
            if ((await types.CountDocumentsAsync(_ => true)) == 0)
            {
                var typeData = await File.ReadAllTextAsync(Path.Combine(SeedBasePath, "types.json"));
                typeList = JsonSerializer.Deserialize<List<ProductType>>(typeData);
                await types.InsertManyAsync(typeList);
            }
            else
            {
                typeList = await types.Find(_ => true).ToListAsync();
            }

            // Seed Products
            if ((await products.CountDocumentsAsync(_ => true)) == 0)
            {
                var productData = await File.ReadAllTextAsync(Path.Combine(SeedBasePath,"products.json"));
                var productList = JsonSerializer.Deserialize<List<Product>>(productData);
                foreach (var product in productList)
                {
                    // Reset Id to let mongo generate one
                    product.Id = null;
                    // Default create date if not set
                    if (product.CreatedDate == default)
                        product.CreatedDate = DateTime.UtcNow; 
                }
                await products.InsertManyAsync(productList);
            }
        }
    }
}
