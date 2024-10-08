using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Airbnb.Domain;
using Airbnb.Domain.DataTransferObjects;
using Airbnb.Domain.Entities;
using Airbnb.Domain.Interfaces.Repositories;
using Airbnb.Domain.Interfaces.Services;
using Airbnb.Infrastructure.Specifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Bcpg;
using Stripe;
using Stripe.Checkout;
using Stripe.TestHelpers;
using RefundService = Stripe.RefundService;

namespace Airbnb.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public PaymentService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<Responses> CreateCheckoutSessioinAsync(int bookingId)
        {

            var booking = await _unitOfWork.Repository<Booking, int>().GetByIdAsync(bookingId);
            if(booking is null) return await Responses.FailurResponse("booking is not found", System.Net.HttpStatusCode.NotFound);
            string BaseUrl = _configuration["BaseUrl"];
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                    "card",
                },

                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount =(long) booking.TotalPrice * 100, // amount in cents (e.g. 20.00 USD)
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = booking.Property.Name,
                            },
                        },
                        Quantity = 1,
                    },
                },

                Mode = "payment",
                SuccessUrl = BaseUrl + $"api/payment/success?bookingId={booking.Id}",
                CancelUrl =  BaseUrl + $"api/payment/cancel?bookingId={booking.Id}",
            };

            var service = new SessionService();
            Session session = service.Create(options);

            // Return the URL to the frontend or redirect user
            return await Responses.SuccessResponse(session.Url);
        }

        public async Task<Responses> CreatePaymentIntentAsync(string currency, int bookingId)
        {
            var booking = await _unitOfWork.Repository<Booking, int>().GetByIdAsync(bookingId);
            if (booking is null) return await Responses.FailurResponse("there is no booking with this id!");

            var Service = new PaymentIntentService();
            PaymentIntent paymentIntent;

            if (string.IsNullOrEmpty(booking.PaymentIntentId))
            {
                var Options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)(booking.TotalPrice * 100),
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>() { "card" }
                };

                paymentIntent = await Service.CreateAsync(Options);
                booking.PaymentIntentId = paymentIntent.Id;
            }
            else
            {
                var Options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)(booking.TotalPrice * 100)
                };

                paymentIntent = await Service.UpdateAsync(booking.PaymentIntentId, Options);

                booking.PaymentIntentId = paymentIntent.Id;
            }


            _unitOfWork.Repository<Booking, int>().Update(booking);

            var Result = await _unitOfWork.CompleteAsync();

            if (Result <= 0) return await Responses.FailurResponse("Error has been occured while updating the paying status!");

            var customer = new CustomerDTO()
            {
                PaymentIntentId = paymentIntent.Id,
                ClientSecret = paymentIntent.ClientSecret
            };
            return await Responses.SuccessResponse(customer);
        }

        public async Task<Responses> PaymentCancelAsync(int bookingId)
        {
            var booking = await _unitOfWork.Repository<Booking, int>().GetByIdAsync(bookingId);
            if (booking is null) return await Responses.FailurResponse("booking is not found", System.Net.HttpStatusCode.NotFound);

            booking.Status = BookingStatus.Canceled;
            _unitOfWork.Repository<Booking, int>().Update(booking);
            var Result = await _unitOfWork.CompleteAsync();
            if (Result <= 0) return await Responses.FailurResponse("Error has been occured while updating", System.Net.HttpStatusCode.InternalServerError);
            return await Responses.SuccessResponse("Payment was canceled by the user, The owner will Take an action to refund your money if needed!");
        }

        public async Task<Responses> PaymentSuccessAsync(int bookingId)
        {
            var booking = await _unitOfWork.Repository<Booking, int>().GetByIdAsync(bookingId);
            if (booking is null) return await Responses.FailurResponse("booking is not found", System.Net.HttpStatusCode.NotFound);

            booking.Status = BookingStatus.PaymentReceived;
            _unitOfWork.Repository<Booking, int>().Update(booking);
            var Result = await _unitOfWork.CompleteAsync();
            if(Result <= 0) return await Responses.FailurResponse("Error has been occured while updating", System.Net.HttpStatusCode.InternalServerError);
            return await Responses.SuccessResponse("Payment was successful");
        }

        public async Task<Responses> RefundPaymentAsync(int bookingId)
        {
            var booking = await _unitOfWork.Repository<Booking, int>().GetByIdAsync(bookingId);

            if (booking is null)
                return await Responses.FailurResponse("Booking not found!", System.Net.HttpStatusCode.NotFound);

            if (string.IsNullOrEmpty(booking.PaymentIntentId))
                return await Responses.FailurResponse("No Payment Intent ID associated with this booking!", System.Net.HttpStatusCode.BadRequest);

            try
            {
                // Create refund options
                var refundOptions = new RefundCreateOptions
                {
                    PaymentIntent = booking.PaymentIntentId,
                    Amount = (long)(booking.TotalPrice * 100), // Refund the full amount in cents
                };

                // Initiate the refund process
                var refundService = new RefundService();
                Refund refund = await refundService.CreateAsync(refundOptions);

                booking.Status = BookingStatus.Canceled; 
                _unitOfWork.Repository<Booking, int>().Update(booking);

                var result = await _unitOfWork.CompleteAsync();

                if (result <= 0)
                    return await Responses.FailurResponse("Failed to update booking status after refund.", System.Net.HttpStatusCode.InternalServerError);

                return await Responses.SuccessResponse(refund);
            }
            catch (StripeException ex)
            {
                // Handle Stripe exceptions
                return await Responses.FailurResponse($"Stripe error: {ex.Message}", System.Net.HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                // Handle general exceptions
                return await Responses.FailurResponse($"An error occurred: {ex.Message}", System.Net.HttpStatusCode.InternalServerError);
            }
        }

    }
}
