using Airbnb.Domain;
using Airbnb.Domain.Entities;
using Airbnb.Domain.Identity;
using Airbnb.Domain.Interfaces.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Net;
using booking = Airbnb.Domain.Entities.Booking;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Airbnb.Infrastructure.Specifications;
using Airbnb.Application.Utility;
using Airbnb.Domain.CachedObjects;
using Airbnb.Domain.DataTransferObjects.User;

namespace Airbnb.Application.Features.Bookings.Command.CreateBooking
{
	public record CreateBookingCommand : IRequest<Responses>
	{
		public string Id { get; set; }
		public string BookingType { get; set; }
		public string PropertyId { get; set; }
		public string UserId { get; set; }
		public PaymentMethod PaymentMethod { get; set; }
		public DateTimeOffset StartDate { get; set; }
		public DateTimeOffset EndDate { get; set; }
	}
	public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, Responses>
	{
		private readonly IValidator<CreateBookingCommand> _validator;
		private readonly IHttpContextAccessor _contextAccessor;
		private readonly UserManager<AppUser> _userManager;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IDistributedCache _cache;
		public CreateBookingCommandHandler(IValidator<CreateBookingCommand> validator,
			IHttpContextAccessor contextAccessor,
			UserManager<AppUser> userManager,
			IUnitOfWork unitOfWork,
			IDistributedCache cache)

		{
			_validator = validator;
			_contextAccessor = contextAccessor;
			_userManager = userManager;
			_unitOfWork = unitOfWork;
			_cache = cache;
		}
		// TODO: Payment Integration
		// TODO: Send Notifications

		public async Task<Responses> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
		{
			var validation = await _validator.ValidateAsync(request);
			if (!validation.IsValid)
			{
				return await Responses.FailurResponse(validation.Errors.ToList());
			}
			
			var user = await GetUser.GetCurrentUserAsync(_contextAccessor,_userManager);
			if (user == null)
			{
				return await Responses.FailurResponse("UnAuthorized user!", HttpStatusCode.Unauthorized);
			}

			var property = await _unitOfWork.Repository<Property, string>().GetByIdAsync(request.PropertyId)!;
			if (property == null)
			{
				return await Responses.FailurResponse($"Not found property with Id {request.PropertyId} !", HttpStatusCode.NotFound);
			}


			var spec = new BookingWithSpec(null, request.PropertyId, request.StartDate, request.EndDate);
			var bookings = await _unitOfWork.Repository<booking, int>().GetAllWithSpecAsync(spec)!;


			if (bookings.Any())
			{
				return await Responses.FailurResponse("This property already booked in this date range!");
			}

			var period = request.EndDate - request.StartDate;

			var totalPrice = property.NightPrice * period.Days;
			var booking = new CachedBooking()
			{
				BookingType = request.BookingType,
				StartDate = request.StartDate,
				EndDate = request.EndDate,
				PaymentMethod = request.PaymentMethod.ToString(),
				PaymentStatus = PaymentStatus.Pending.ToString(),
				BookingDate = DateTimeOffset.UtcNow,
				UserId = user.Id,
				User=new OwnerDto
				{
					FullName=user.FullName,
					Email=user.Email!,
					PhoneNumber=user.PhoneNumber!
				},
				PropertyId = property.Id,
				TotalPrice=totalPrice
			};
		
			try
			{
				var jsonData = JsonSerializer.Serialize(booking);

				var options = new DistributedCacheEntryOptions
				{
					AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(5)
				};

				await _cache.SetStringAsync(request.Id, jsonData, options);

				//TODO: SendNotification here 
				return await Responses.SuccessResponse($"Property with id {property.Id} has been booked successfully!");
			}
			catch (Exception ex)
			{
				return await Responses.FailurResponse(ex.Message, HttpStatusCode.InternalServerError);
			}
		}

	}
}
