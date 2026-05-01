using Basket.DTOs;
using MediatR;

namespace Basket.Commands
{
    public record BasketCheckoutCommand(BasketCheckoutDto Dto) : IRequest<Unit>;
  
}
