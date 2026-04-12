using Ordering.Commands;
using Ordering.DTOs;
using Ordering.Entities;

namespace Ordering.Mappers
{
    public static class OrderMapper
    {
        public static OrderDto ToDto(this Order order) =>
            new(order.Id, order.UserName, order.TotalPrice ?? 0, order.FirstName!, order.LastName!,
                order.EmailAddress!, order.AddressLine!, order.Country!, order.State!, order.ZipCode!,
                order.CardName!,order.CardNumber, order.Expiration!, order.Cvv!, order.PaymentMethd ?? 0);

        public static Order ToEntity(this CheckoutOrderCommand command)
        {
            return new Order
            {
                UserName = command.UserName,
                TotalPrice = command.TotalPrice,
                FirstName = command.FirstName,
                LastName = command.LastName,
                EmailAddress = command.EmailAddress,
                AddressLine = command.AddressLine,
                Country = command.Country,
                State = command.State,
                ZipCode = command.ZipCode,
                CardName = command.CardName,
                CardNumber = command.CardNumber,
                Expiration = command.Expiration,
                Cvv = command.Cvv,
                PaymentMethd = command.PaymentMethd,
            };
        }
    }
}
