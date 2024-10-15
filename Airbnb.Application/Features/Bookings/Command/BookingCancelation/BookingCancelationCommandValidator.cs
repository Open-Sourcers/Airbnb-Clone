using Airbnb.Domain;
using Airbnb.Domain.Entities;
using Airbnb.Domain.Interfaces.Repositories;
using FluentValidation;
using MediatR;
namespace Airbnb.Application.Features.Bookings.Command.BookingCancelation
{
    public class BookingCancelationCommandValidator : AbstractValidator<BookingCancelationCommand>
    {
        public BookingCancelationCommandValidator()
        {
            RuleFor(x => x.bookingId).NotNull()
                .Must(bookingId => int.TryParse(bookingId.ToString(), out _));
        }
    }
}
