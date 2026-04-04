using Basket.Entities;

namespace Basket.Repositories
{
    public interface IBasketRepository
    {
        Task<ShoppingCart> GetBasket(string userName);
        Task<ShoppingCart> UpdateUpsertBasket(ShoppingCart shoppingCart);
        Task DeleteBasket(string userName);
    }
}
