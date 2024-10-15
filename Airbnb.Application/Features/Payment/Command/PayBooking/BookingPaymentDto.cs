using Airbnb.Domain.CachedObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airbnb.Application.Features.Payment.Command.PayBooking
{
	public class BookingPaymentDto : CachedBooking
	{
		public string PaymentIntentId { get; set; } = string.Empty;
		public string ClientSecret { get; set; } = string.Empty;
	}
}
