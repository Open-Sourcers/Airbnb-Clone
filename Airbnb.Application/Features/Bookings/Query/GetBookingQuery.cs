using Airbnb.Application.Utility;
using Airbnb.Domain;
using Airbnb.Domain.CachedObjects;
using Airbnb.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using System.Net;
using System.Text.Json;
using booking = Airbnb.Domain.Entities.Booking;

namespace Airbnb.Application.Features.Bookings.Query
{
	public record GetBookingQuery : IRequest<Responses>
	{
		public string BookingId { get; set; }
		public GetBookingQuery(string bookingId)
		{
			BookingId = bookingId;
		}
	}
	public class GetBookingQueryHandler : IRequestHandler<GetBookingQuery, Responses>
	{
		private readonly IHttpContextAccessor _contextAccessor;
		private readonly UserManager<AppUser> _userManager;
		private readonly IDistributedCache _cache;

		public GetBookingQueryHandler(IHttpContextAccessor contextAccessor,
			UserManager<AppUser> userManager,
			IDistributedCache cache)
		{
			_contextAccessor = contextAccessor;
			_userManager = userManager;
			_cache = cache;

		}

		public async Task<Responses> Handle(GetBookingQuery request, CancellationToken cancellationToken)
		{


			var jsonData = await _cache.GetStringAsync(request.BookingId);
			if (jsonData == null)
			{
				return await Responses.FailurResponse($"Booking with Id {request.BookingId} not found", HttpStatusCode.NotFound);
			}
			try
			{
				var booking = JsonSerializer.Deserialize<CachedBooking>(jsonData);
				var user = await GetUser.GetCurrentUserAsync(_contextAccessor, _userManager);

				if (user == null || booking!.UserId != user.Id)
				{
					return await Responses.FailurResponse("UnAuthorized user!", HttpStatusCode.Unauthorized);
				}
				
				return await Responses.SuccessResponse(booking);
			}catch(Exception ex)
			{
				return await Responses.FailurResponse(ex.Message);
			}
		}
	}

}
