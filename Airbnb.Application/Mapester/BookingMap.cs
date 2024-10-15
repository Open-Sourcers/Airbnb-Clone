using Airbnb.Application.Features.Bookings.Query;
using Airbnb.Domain.Entities;
using Mapster;
namespace Airbnb.Application.Mapester
{
    public class BookingMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Booking, GetBookingQueryDto>();
        }
    }
}
