using Dapper;
using Discount.Entities;
using Npgsql;

namespace Discount.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly string? _configuration;

        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration.GetValue<string>("DatabaseSettings:ConnectionString");
        }

        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            await using var connection = new NpgsqlConnection(_configuration);
            var affected = await connection.ExecuteAsync(
                "INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)",
                new {coupon.ProductName,coupon.Description, coupon.Amount}
                );
            return affected > 0;
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            await using var connection = new NpgsqlConnection(_configuration);
            var affected = await connection.ExecuteAsync("DELETE FROM Coupon WHERE ProductName = @ProductName",
                new {ProductName = productName});
            return affected > 0;    
        }

        public async Task<Coupon> GetDiscount(string productName)
        {
            await using var connection = new NpgsqlConnection(_configuration);
            var coupon = await connection.QueryFirstAsync<Coupon>(
                "SELECT * FROM Coupon WHERE ProductName = @ProductName",
                new {ProductName = productName}
                );

            return coupon ?? new Coupon { 
                ProductName = "No Discount",
                Amount = 0,
                Description = "No Discount Available"
            };
        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            await using var connection = new NpgsqlConnection(_configuration);
            var affected = await connection.ExecuteAsync(
                "UPDATE Coupon SET ProductName = @ProductName, Description = @Description, Amount = @Amount WHERE Id =@Id",
                new {coupon.ProductName, coupon.Description, coupon.Amount, coupon.Id}
                );
            return affected > 0;
        }
    }
}
