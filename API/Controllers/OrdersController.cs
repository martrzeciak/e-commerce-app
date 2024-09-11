using API.DTOs;
using API.Extensions;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class OrdersController(IUnitOfWork unitOfWork, ICartService cartService) 
        : BaseApiController
    {
        [HttpPost]
        public async Task<ActionResult<OrderDto>> CreateOrder(CreateOrderDto orderDto)
        {
            var email = User.GetEmail();

            var cart = await cartService.GetCartAsync(orderDto.CartId);

            if (cart == null) return BadRequest("Cart not found");

            if (cart.PaymentIntentId == null) 
                return BadRequest("No payment intent for this order");

            var items = new List<OrderItem>();

            foreach (var item in cart.Items)
            {
                var productItem = await unitOfWork.Repository<Product>()
                    .GetByIdAsync(item.ProductId);

                if (productItem == null) return BadRequest("Problem with order");

                var itmeOrdered = new ProductItemOrdered
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    PictureUrl = item.PictureUrl
                };

                var orderItem = new OrderItem
                {
                    ItemOrdered = itmeOrdered,
                    Price = productItem.Price,
                    Quantity = item.Quantity
                };

                items.Add(orderItem);
            }

            var deliveryMethod = await unitOfWork.Repository<DeliveryMethod>()
                .GetByIdAsync(orderDto.DeliveryMethodId);

            if (deliveryMethod == null) return BadRequest("No delivery method selected");


            var order = new Order
            {
                OrderItems = items,
                DeliveryMethod = deliveryMethod,
                ShippingAddress = orderDto.ShippingAddress.ToEntity(),
                Subtotal = items.Sum(x => x.Quantity * x.Price),
                PaymentSummary = orderDto.PaymentSummary.ToEntity(),
                PaymentIntentId = cart.PaymentIntentId,
                BuyerEamil = email
            };

            unitOfWork.Repository<Order>().Add(order);

            if (await unitOfWork.Complete())
            {
                return order.ToDto();
            }

            return BadRequest("Problem creating order");
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderDto>>> GetOrdersForUser()
        {
            var spec = new OrderSpecyfication(User.GetEmail());

            var orders = await unitOfWork.Repository<Order>().ListAsync(spec);

            var ordersToReturn = orders.Select(x => x.ToDto()).ToList(); 

            return Ok(ordersToReturn);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderDto>> GetOrderById(int id)
        {
            var spec = new OrderSpecyfication(User.GetEmail(), id);

            var order = await unitOfWork.Repository<Order>().GetEntityWithSpec(spec);

            if (order == null) return NotFound();

            return order.ToDto();
        }
    }
}
