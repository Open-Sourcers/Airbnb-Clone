using Airbnb.Domain.DataTransferObjects.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airbnb.Domain.CachedObjects
{
	public class CachedBooking
	{
		public string BookingType { get; set; } = string.Empty;
		public decimal TotalPrice { get; set; }
		public DateTimeOffset StartDate { get; set; }
		public DateTimeOffset EndDate { get; set; }
		public string PaymentMethod { get; set; } = string.Empty;
		public DateTimeOffset BookingDate { get; set; } = DateTimeOffset.Now;
		public string PaymentStatus { get; set; } = string.Empty;
		public string PropertyId { get; set; } = string.Empty;
		public string UserId { get; set; }=string.Empty;
		public OwnerDto User { get; set; }
	}
}
