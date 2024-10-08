using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Airbnb.Domain.DataTransferObjects;
using Airbnb.Domain.Entities;
using Stripe;
using Stripe.Climate;

namespace Airbnb.Domain.Interfaces.Services
{
    public interface IPaymentService
    {
        Task<Responses> CreatePaymentIntentAsync(string currency, int bookingId);
        Task<Responses> CreateCheckoutSessioinAsync(int bookingId);
        Task<Responses> RefundPaymentAsync(int bookingId);
        Task<Responses> PaymentSuccessAsync(int bookingId);
        Task<Responses> PaymentCancelAsync(int bookingId);
    }
}
