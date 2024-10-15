using Airbnb.Application.Utility;
using Airbnb.Domain;
using Airbnb.Domain.CachedObjects;
using Airbnb.Domain.Entities;
using Airbnb.Domain.Identity;
using Airbnb.Domain.Interfaces.Repositories;
using Airbnb.Infrastructure.Specifications;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Org.BouncyCastle.Security;
using Stripe;
using System.Net;
using System.Text.Json;

namespace Airbnb.Application.Features.Payment.Command.RegisterBooking
{
	public record RegisterBookingPaymentCommand : IRequest<Responses>
	{
		public string BookingId { get; set; }
		public string PaymentIntentId { get; set; }

	}
	public class RegisterBookingPaymentCommandHandler : IRequestHandler<RegisterBookingPaymentCommand, Responses>
	{
		private readonly IDistributedCache _cache;
		private readonly IHttpContextAccessor _contextAccessor;
		private readonly UserManager<AppUser> _userManager;
		private readonly IUnitOfWork _unitOfWork;
		public RegisterBookingPaymentCommandHandler(IDistributedCache cache, IHttpContextAccessor contextAccessor, UserManager<AppUser> userManager, IUnitOfWork unitOfWork)
		{
			_cache = cache;
			_contextAccessor = contextAccessor;
			_userManager = userManager;
			_unitOfWork = unitOfWork;
		}

		public async Task<Responses> Handle(RegisterBookingPaymentCommand request, CancellationToken cancellationToken)
		{
			var jsonBooking = await _cache.GetStringAsync(request.BookingId);
			if (jsonBooking == null)
			{
				return await Responses.FailurResponse($"Booking with Id {request.BookingId} not found!", HttpStatusCode.NotFound);
			}
			var booking = JsonSerializer.Deserialize<CachedBooking>(jsonBooking);
			var user = await GetUser.GetCurrentUserAsync(_contextAccessor, _userManager);
			if (user == null || user.Id != booking.UserId)
			{
				return await Responses.FailurResponse("UnAuthorized!", HttpStatusCode.Unauthorized);
			}

			var spec = new BookingWithSpec(paymentIntentId: request.PaymentIntentId);
			var paymentIntentIdIsValid = await _unitOfWork.Repository<Booking, int>().GetEntityWithSpecAsync(spec)!;
			if (paymentIntentIdIsValid != null)
			{
				return await Responses.FailurResponse($"PaymentIntentId {request.PaymentIntentId} invalid!", HttpStatusCode.BadRequest);
			}

			var paymentData = new Booking()
			{
				Name = booking.BookingType,
				TotalPrice = booking.TotalPrice,
				StartDate = booking.StartDate,
				EndDate = booking.EndDate,
				PaymentDate = DateTimeOffset.UtcNow,
				BookingDate = booking.BookingDate,
				PaymentMethod = booking.PaymentMethod,
				PaymentStatus = booking.PaymentStatus,
				PaymentIntentId = request.PaymentIntentId,
				PropertyId = booking.PropertyId,
				UserId = booking.UserId,
			};

			try
			{
				await _unitOfWork.Repository<Booking, int>().AddAsync(paymentData);
				await _unitOfWork.CompleteAsync();

				_cache.Remove(request.BookingId);

				return await Responses.SuccessResponse("Booking data has been registered successfully!");
			}
			catch (Exception ex)
			{
				return await Responses.FailurResponse(ex.Message, HttpStatusCode.InternalServerError);
			}


		}
	}
}
