using API.DTOs;
using Core.Entities.OrderAggregate;

namespace API.Extensions
{
    public static class OrderMappingExtensions
    {
        public static OrderDto ToDto(this Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                BuyerEamil = order.BuyerEamil,
                OrderDate = order.OrderDate,
                ShippingAddress = order.ShippingAddress.ToDto(),
                PaymentSummary = order.PaymentSummary.ToDto(),
                DeliveryMethod = order.DeliveryMethod.Description,
                ShippingPrice = order.DeliveryMethod.Price,
                OrderItems = order.OrderItems.Select(x => x.ToDto()).ToList(),
                Subtotal = order.Subtotal,
                Total = order.GetTotal(),
                Status = order.Status.ToString(),
                PaymentIntentId = order.PaymentIntentId
            };
        }

        public static OrderItemDto ToDto(this OrderItem orderItem)
        {
            return new OrderItemDto
            {
                ProductId = orderItem.ItemOrdered.ProductId,
                ProductName = orderItem.ItemOrdered.ProductName,
                PictureUrl = orderItem.ItemOrdered.PictureUrl,
                Price = orderItem.Price,
                Quantity = orderItem.Quantity
            };
        }
    }
}


