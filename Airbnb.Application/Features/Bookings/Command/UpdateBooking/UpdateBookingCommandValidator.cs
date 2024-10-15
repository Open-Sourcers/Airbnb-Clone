using Airbnb.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airbnb.Application.Features.Bookings.Command.UpdateBooking
{
	public class UpdateBookingCommandValidator:AbstractValidator<UpdateBookingCommand>
	{
        public UpdateBookingCommandValidator()
        {
            RuleFor(x => x.BookingId).NotNull();
            RuleFor(x => x.PaymentMethod).NotNull().Must(method => method == PaymentMethod.Cache || method == PaymentMethod.Visa);
            RuleFor(x => x.BookingType).NotNull();
        }
    }
}
