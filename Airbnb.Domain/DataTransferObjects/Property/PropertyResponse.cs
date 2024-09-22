using Airbnb.Domain.DataTransferObjects.Booking;
using Airbnb.Domain.DataTransferObjects.Category;
using Airbnb.Domain.DataTransferObjects.Country;
using Airbnb.Domain.DataTransferObjects.Image;
using Airbnb.Domain.DataTransferObjects.Location;
using Airbnb.Domain.DataTransferObjects.Region;
using Airbnb.Domain.DataTransferObjects.User;

namespace Airbnb.Domain.DataTransferObjects.Property
{
    public class PropertyResponse:PropertyDto
    {
        public OwnerDto Owner { get; set; }
        public IEnumerable<ImageResponse> Images { get; set; }
        public IEnumerable<BookingResponse> Bookings { get; set; }
        public IEnumerable<ReviewDto> Reviews { get; set; }
        public IEnumerable<CategoryResponse> Categories { get; set; }
        public IEnumerable<RoomServicesDto> RoomServices { get; set; }
    }

}
