using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Airbnb.Domain.Entities;

namespace Airbnb.Infrastructure.Specifications
{
    public class BookingWithPaymentIntentSpecification : BaseSpecifications<Booking, int>
    {
        public BookingWithPaymentIntentSpecification(string PaymentIntentId) : base(B => B.PaymentIntentId == PaymentIntentId)
        {

        }
    }
}
