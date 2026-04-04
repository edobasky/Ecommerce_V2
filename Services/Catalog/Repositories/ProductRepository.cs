using Catalog.Entities;
using Catalog.Specification;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMongoCollection<Product> _products;
        private readonly IMongoCollection<ProductBrand> _brands;
        private readonly IMongoCollection<ProductType> _types;
        public ProductRepository(IConfiguration config)
        {
            var client = new MongoClient(config["DatabaseSettings:ConnectionString"]);
            var db = client.GetDatabase(config["DatabaseSettings:DatabaseName"]);
            _products = db.GetCollection<Product>(config["DatabaseSettings:ProductCollectionName"]);
            _brands = db.GetCollection<ProductBrand>(config["DatabaseSettings:BrandCollectionName"]);
            _types = db.GetCollection<ProductType>(config["DatabaseSettings:TypeCollectionName"]);


        }

        public async Task<Product> CreateProduct(Product product)
        {
            await _products.InsertOneAsync(product);
            return product;
        }

        public async Task<bool> DeleteProduct(string id)
        {
            var deletedProduct = await _products.DeleteOneAsync(p => p.Id == id);
            return deletedProduct.IsAcknowledged && deletedProduct.DeletedCount > 0;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _products.Find(p => true).ToListAsync();
        }

        public async Task<ProductBrand> GetBrandByIdAsync(string brandId)
        {
            return await _brands.Find(x => x.Id == brandId).FirstOrDefaultAsync(); ;
        }

        public async Task<Product> GetProduct(string productId)
        {
            return await _products.Find(p => p.Id == productId).FirstOrDefaultAsync();
        }

        public async Task<Pagination<Product>> GetProducts(CatalogSpecParams specParams)
        {
            var builder = Builders<Product>.Filter;
            var filter = builder.Empty;
            if (!string.IsNullOrEmpty(specParams.Search))
            {
                filter &= builder.Where(p => p.Name.ToLower().Contains(specParams.Search.ToLower()));
            }
            if (!string.IsNullOrEmpty(specParams.BrandId))
            {
                filter &= builder.Eq(p => p.Brand.Id, specParams.BrandId);
            }
            if (!string.IsNullOrEmpty(specParams.TypeId))
            {
                filter &= builder.Eq(p => p.Type.Id, specParams.TypeId);
            }

            var totalItems = await _products.CountDocumentsAsync(filter);
            var data = await ApplyDataFilters(specParams, filter);

            return new Pagination<Product>(
                specParams.PageIndex, specParams.PageSize,
                (int)totalItems, data);
        }


        public async Task<IEnumerable<Product>> GetProductsByBrand(string name)
        {
            return await _products
                .Find(p => p.Brand.Name.ToLower() == name.ToLower())
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByName(string name)
        {
            var filter = Builders<Product>.Filter.Regex(p => p.Name, new BsonRegularExpression($".*{name}.*","i"));
            return await _products.Find(filter).ToListAsync();
        }

        public async Task<ProductType> GetTypeByTypeIdAsync(string typeId)
        {
            return await _types.Find(t => t.Id == typeId).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var updateProduct = await _products
                .ReplaceOneAsync(p => p.Id == product.Id,product);
            return updateProduct.IsAcknowledged && updateProduct.ModifiedCount > 0;
        }


        private async Task<IReadOnlyCollection<Product>> ApplyDataFilters(CatalogSpecParams specParams, FilterDefinition<Product> filter)
        {
            var sortDefn = Builders<Product>.Sort.Ascending("Name");
            if (!string.IsNullOrEmpty(specParams.Sort))
            {
                sortDefn = specParams.Sort switch
                {
                    "priceAsc" => Builders<Product>.Sort.Ascending(p => p.Price),
                    "priceDesc" => Builders<Product>.Sort.Descending(p => p.Price),
                    _ => Builders<Product>.Sort.Ascending(p => p.Name)
                };
            }
            return await _products
                .Find(filter)
                .Sort(sortDefn)
                .Skip((specParams.PageIndex - 1) * specParams.PageSize)
                .Limit(specParams.PageSize)
                .ToListAsync();
        }
    }
}
