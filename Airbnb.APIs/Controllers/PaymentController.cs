using Airbnb.Application.Utility;
using Airbnb.Domain;
using Airbnb.Domain.Entities;
using Airbnb.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
namespace Airbnb.APIs.Controllers
{
    public class PaymentController : APIBaseController
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("create-payment-intent")]
        public async Task<ActionResult> CreatePaymentIntent([FromQuery] int bookId)
        {
            var paymentIntent = await _paymentService.CreatePaymentIntentAsync("usd", bookId);
            return Ok(paymentIntent);
        }
        // stripe tests
        [HttpGet("success")]
        public async Task<ActionResult<Responses>> PaymentSuccess([FromQuery] int bookingId)
        {
            var Response = await _paymentService.PaymentSuccessAsync(bookingId);
            return Ok(Response);
        }

        [HttpGet("cancel")]
        public  async Task<ActionResult<Responses>> PaymentCancel([FromQuery] int bookingId)
        {
            var Response = await _paymentService.PaymentCancelAsync(bookingId);
            return Ok(Response);
        }

        [HttpPost("create-checkout-session")]
        public async Task<ActionResult<Responses>> CreateCheckoutSession(int bookingId)
        {
            var session = await _paymentService.CreateCheckoutSessioinAsync(bookingId);
            return Ok(session);
        }

        //[Authorize(Roles = "Owner")]
        [HttpPost("refund-payment")]
        public async Task<ActionResult<Responses>> RefundPayment(int bookingId)
        {
            var Refund = await _paymentService.RefundPaymentAsync(bookingId);
            return Ok(Refund);
        }
    }
}
