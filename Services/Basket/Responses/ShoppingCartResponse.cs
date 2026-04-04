namespace Basket.Responses
{
    public record class ShoppingCartResponse
    {
        public string UserName { get; init; }
        public List<ShoppingCartItemResponse> Items { get; init; } = new();
    
        // Default Ctor
        public ShoppingCartResponse()
        {
            UserName = string.Empty;
            Items = new List<ShoppingCartItemResponse>();
        }
        //ctor with username only
        public ShoppingCartResponse(string userName) : this(userName, new List<ShoppingCartItemResponse>())
        {
            UserName = string.Empty;
            Items = new List<ShoppingCartItemResponse>();
        }
        // full Ctor
        public ShoppingCartResponse(string userName, List<ShoppingCartItemResponse> items)
        {
            UserName = userName;
            Items = items ?? new List<ShoppingCartItemResponse>();
        }

        public decimal TotalPrice => Items.Sum(item => item.Price * item.Quantity);

    }
   
}
 