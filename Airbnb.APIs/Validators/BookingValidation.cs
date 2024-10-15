using Airbnb.Domain.DataTransferObjects.Booking;
using FluentValidation;

namespace Airbnb.APIs.Validators
{
    public class BookingValidation:AbstractValidator<BookingRequest>
    {
        public BookingValidation()
        {
            RuleFor(x => x.PropertyId).NotNull();
            RuleFor(x=>x.UserId).NotNull();
            RuleFor(x => x.StartDate).NotNull();
            RuleFor(x => x.EndDate).NotNull();
            RuleFor(x => x.PaymentMethod).NotNull();
        }
    }
}
