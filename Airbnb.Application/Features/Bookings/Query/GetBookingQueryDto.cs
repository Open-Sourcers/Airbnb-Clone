
using Airbnb.Domain.DataTransferObjects.User;
using Airbnb.Domain.Entities;

namespace Airbnb.Application.Features.Bookings.Query
{
    public class GetBookingQueryDto
    {
        public string PropertyId { get; set; }
        public string UserId { get; set; }
        public OwnerDto User { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public string PaymentMethod { get; set; } 
        public decimal TotalPrice { get; set; }
    }
}
