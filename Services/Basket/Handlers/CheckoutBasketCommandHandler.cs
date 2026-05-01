using Basket.Commands;
using MediatR;

namespace Basket.Handlers
{
    public class CheckoutBasketCommandHandler : IRequestHandler<BasketCheckoutCommand, Unit>
    {
        public Task<Unit> Handle(BasketCheckoutCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
