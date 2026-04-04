using Basket.Commands;
using Basket.Entities;
using Basket.Responses;

namespace Basket.Mappers
{
    public static class BasketMapper
    {
        public static ShoppingCartResponse ToResponse(this ShoppingCart shoppingCart)
        {
            return new ShoppingCartResponse
            {
                UserName = shoppingCart.UserName,
                Items = shoppingCart.Items.Select(x => new ShoppingCartItemResponse
                {
                    Quantity = x.Quantity,
                    ImageFile = x.ImageFile,
                    Price = x.Price,
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                }).ToList()
            };
        }

        public static ShoppingCart ToEntity(this CreateShoppingCartCommand command)
        {
            return new ShoppingCart
            {
                UserName = command.UserName,
                Items = command.Items.Select(item => new ShoppingCartItem
                {
                    Quantity = item.Quantity,
                    ImageFile = item.ImageFile,
                    Price = item.Price,
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                }).ToList()
            };
        }
    }
}
