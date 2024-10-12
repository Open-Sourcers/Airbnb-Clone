namespace Airbnb.Domain.DataTransferObjects
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string PhoneNumber { get; set; }
        public IEnumerable<BookingDto> Bookings { get; set; }
        public IEnumerable<PropertyUserDto> Properties { get; set; }
        public IEnumerable<ReviewDto> Reviews { get; set; }
        public string? profileImage { get; set; }
    }
    public class BookingDto
    {
        public decimal TotalPrice { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public DateTimeOffset PaymentDate { get; set; }
    }
    public class PropertyUserDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal NightPrice { get; set; }
        public float Rate { get; set; }
        public string PlaceType { get; set; } = string.Empty;

    }
    public class ReviewDto
    {
        public string Comment { get; set; } = string.Empty;
        public int Stars { get; set; }

    }
}
