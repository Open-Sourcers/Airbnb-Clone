using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Airbnb.Domain.Entities;

namespace Airbnb.Infrastructure.Specifications
{
    public class BookingSpecifications : BaseSpecifications<Booking, int>
    {
        public BookingSpecifications(string userId) : base(B => B.UserId == userId)
        {
            
        }
    }
}
