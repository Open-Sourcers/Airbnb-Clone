namespace Airbnb.Domain.DataTransferObjects.Booking
{
    public class BookingResponse: BookingDto
    {
        public decimal TotalPrice { get; set; }
        public DateTimeOffset PaymentDate { get; set; }

    }
}
