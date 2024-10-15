using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airbnb.Application.Features.Payment.Command.RegisterBooking
{
	public class RegisterBookingCommandValidator:AbstractValidator<RegisterBookingPaymentCommand>
	{
        public RegisterBookingCommandValidator()
        {
            RuleFor(x => x.BookingId).NotNull();
            RuleFor(x=>x.PaymentIntentId).NotNull();
        }
    }
}
