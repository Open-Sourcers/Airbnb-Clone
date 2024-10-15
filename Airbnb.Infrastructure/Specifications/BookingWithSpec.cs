

using Airbnb.Domain.Entities;
using System.Collections.Immutable;

namespace Airbnb.Infrastructure.Specifications
{
	public class BookingWithSpec : BaseSpecifications<Booking, int>
	{
		public BookingWithSpec(string? userId = null, string? propertyId = null, DateTimeOffset? startDate = null, DateTimeOffset? EndDate = null, string? paymentIntentId = null) :
			base(B =>
				(string.IsNullOrWhiteSpace(userId) || B.UserId == userId) &&
				(string.IsNullOrWhiteSpace(userId) || B.PropertyId == propertyId) &&
				(!(startDate != null && EndDate != null) ||
					(startDate >= B.StartDate && startDate <= B.EndDate) ||
					(EndDate >= B.StartDate && EndDate <= B.EndDate)) &&
			    (string.IsNullOrWhiteSpace(paymentIntentId) || B.PaymentIntentId == paymentIntentId)

				)
		{
			AddOrderByDescending(x => x.BookingDate);
		}
	}
}
