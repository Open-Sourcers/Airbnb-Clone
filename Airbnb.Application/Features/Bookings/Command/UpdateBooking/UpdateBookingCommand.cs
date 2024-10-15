using Airbnb.Application.Utility;
using Airbnb.Domain;
using Airbnb.Domain.CachedObjects;
using Airbnb.Domain.Entities;
using Airbnb.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using System.Net;
using System.Text.Json;

namespace Airbnb.Application.Features.Bookings.Command
{
	public record UpdateBookingCommand : IRequest<Responses>
	{
		public string BookingId { get; set; }
		public string BookingType { get; set; }
		public PaymentMethod PaymentMethod { get; set; }
	}
	public class UpdateBookingCommandHandler : IRequestHandler<UpdateBookingCommand, Responses>
	{
		private readonly IHttpContextAccessor _contextAccessor;
		private readonly UserManager<AppUser> _userManager;
		private readonly IDistributedCache _cache;
		public UpdateBookingCommandHandler(IHttpContextAccessor contextAccessor,
			UserManager<AppUser> userManager,
			IDistributedCache cache)
		{
			_contextAccessor = contextAccessor;
			_userManager = userManager;
			_cache = cache;
		}

		public async Task<Responses> Handle(UpdateBookingCommand request, CancellationToken cancellationToken)
		{
			var user = GetUser.GetCurrentUserAsync(_contextAccessor, _userManager);
			if (user == null) return await Responses.FailurResponse("UnAuthorized!", HttpStatusCode.Unauthorized);

			var jsonData = await _cache.GetStringAsync(request.BookingId);
			if (jsonData == null) return await Responses.FailurResponse($"Booking with Id {request.BookingId} not found!", HttpStatusCode.NotFound);

			var booking = JsonSerializer.Deserialize<CachedBooking>(jsonData);
			booking!.BookingType = request.BookingType;
			booking.PaymentMethod = request.PaymentMethod.ToString();
			var updated = JsonSerializer.Serialize(booking);

			var options = new DistributedCacheEntryOptions()
			{
				AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(2)
			};
			await _cache.SetStringAsync(request.BookingId, updated, options);
			// TODO: Send Notification here for period 
			return await Responses.SuccessResponse($"Booking with id {request.BookingId} updated successfully.");
		}
	}
}
