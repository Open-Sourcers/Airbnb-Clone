using Airbnb.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airbnb.Application.Features.Bookings.Command.CreateBooking
{
	public class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
	{
		public CreateBookingCommandValidator()
		{
			RuleFor(x => x.Id).NotNull().Length(1, 50);
			RuleFor(x => x.BookingType).NotNull().Length(3, 20);
			RuleFor(x => x.PropertyId).NotNull();
			RuleFor(x => x.UserId).NotNull();
			RuleFor(x => x.PaymentMethod).NotNull().Must(pm => pm == PaymentMethod.Cache || pm == PaymentMethod.Visa);
			RuleFor(x => x.StartDate).NotNull();
			RuleFor(x => x.EndDate).NotNull().GreaterThan(x => x.StartDate);
		}
	}
}
