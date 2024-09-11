using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace API.Controllers
{
    public class PaymentsController(IPaymentService paymentService,
        IUnitOfWork unitOfWork, ILogger<PaymentsController> logger, IConfiguration config)
            : BaseApiController
    {
        private readonly string _webhookSecret = config["StripeSettings:WebhookSecret"]!;

        [Authorize]
        [HttpPost("{cartId}")]
        public async Task<ActionResult<ShoppingCart>> CreateOrUpdatePaymentIntent(string cartId)
        {
            var cart = await paymentService.CreateOrUpdatePaymentIntent(cartId);

            if (cart == null) return BadRequest("Problem with your cart");

            return Ok(cart);
        }

        [HttpGet("delivery-method")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            return Ok(await unitOfWork.Repository<DeliveryMethod>().ListAllAsync());
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(Request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = ConstructStripeEvenet(json);

                if (stripeEvent.Data.Object is not PaymentIntent intent)
                    return BadRequest("Invalid event data");

                await HandlePaymentIntentSucceeded(intent);

                return Ok();
            }
            catch (StripeException ex)
            {
                logger.LogError(ex, "Stripe webhook error");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Stripe webhook error");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unexpected error occurred");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred");
            }
        }

        private async Task HandlePaymentIntentSucceeded(PaymentIntent intent)
        {
            if (intent.Status == "succeeded")
            {
                var spec = new OrderSpecyfication(intent.Id, true);

                var order = await unitOfWork.Repository<Order>().GetEntityWithSpec(spec)
                    ?? throw new Exception("Order not found");

                if ((long)order.GetTotal() * 100 != intent.Amount)
                    order.Status = OrderStatus.PaymentMismatch;
                else
                    order.Status = OrderStatus.PaymentReceived;

                await unitOfWork.Complete();
            }
        }

        private Event ConstructStripeEvenet(string json)
        {
            try
            {
                return EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], _webhookSecret);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to construct stripe event");
                throw new StripeException("Invalid signature");
            }
        }
    }
}
