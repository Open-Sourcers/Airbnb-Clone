using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Airbnb.Domain;
using Airbnb.Domain.DataTransferObjects;
using Airbnb.Domain.Entities;
using Airbnb.Domain.Identity;
using Airbnb.Domain.Interfaces.Repositories;
using Airbnb.Domain.Interfaces.Services;
using Airbnb.Infrastructure.Repositories;
using Airbnb.Infrastructure.Specifications;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Property = Airbnb.Domain.Entities.Property;

namespace Airbnb.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IPaymentService _paymentService;

        public BookService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, IMapper mapper, IPaymentService paymentService)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
            _paymentService = paymentService;
        }

        public async Task<Responses> CreateBookingByPropertyId(string email, BookingToCreateDTO bookDTO)
        {
            var property = await _unitOfWork.Repository<Property, string>().GetByIdAsync(bookDTO.PropertyId);
            if (property == null) return await Responses.FailurResponse("Property not found!");

            bool isAvailable = await _unitOfWork.Repository<Booking, int>().CheckAvailabilityAsync(b => b.PropertyId == property.Id, bookDTO.StartDate, bookDTO.EndDate);
            if(!isAvailable) return await Responses.FailurResponse("please check another date, property isn't availabe at this time!");

            var user = await _userManager.FindByEmailAsync(email);
            if (user is null) return await Responses.FailurResponse("please login to proccess!");

            var totalPrice = (bookDTO.EndDate.Day - bookDTO.StartDate.Day) * property.NightPrice;

             //var intent = await _paymentService.CreatePaymentIntent(totalPrice, "usd", );

             var booking = new Booking
             {
                 PropertyId = property.Id,
                 StartDate = bookDTO.StartDate,
                 EndDate = bookDTO.EndDate,
                 TotalPrice = totalPrice,
                 UserId = user.Id,
                 PaymentDate = DateTime.Now,
                 PaymentMethod = "card",
             };

            await _unitOfWork.Repository<Booking, int>().AddAsync(booking);
            var Result = await _unitOfWork.CompleteAsync();
            if (Result <= 0) return await Responses.FailurResponse("Error has been occured while booking the property!");

            return await Responses.SuccessResponse("Property has been booked successfully!");
        }

        public async Task<Responses> DeleteBookingById(int bookingId)
        {
            var booking = await _unitOfWork.Repository<Booking, int>().GetByIdAsync(bookingId);
            if (booking == null) return await Responses.FailurResponse("this book is not found or there is error!");
            // implement refund money if the date not passed yet
            _unitOfWork.Repository<Booking, int>().Remove(booking);
            var Result = await _unitOfWork.CompleteAsync();
            if (Result <= 0) return await Responses.FailurResponse("Error has been occured while removing");
            return await Responses.SuccessResponse("Booking has been deleted successfully!");
        }

        public async Task<Responses> GetBookingById(int bookingId)
        {
            var booking = await _unitOfWork.Repository<Booking, int>().GetByIdAsync(bookingId);
            if (booking == null) return await Responses.FailurResponse("this book is not found or there is error!");
            var MappedBooking = _mapper.Map<Booking, BookingDto>(booking);
            return await Responses.SuccessResponse(MappedBooking);
        }

        public async Task<Responses> GetBookingsByUserId(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null) return await Responses.FailurResponse("please login to proccess!");
            var spec = new BookingSpecifications(user.Id);
            var bookings = await _unitOfWork.Repository<Booking, int>().GetAllWithSpecAsync(spec);
            if(bookings is null) return await Responses.FailurResponse("There is no booking for you!");
            var MappedBookings = _mapper.Map<IReadOnlyList<Booking>, IReadOnlyList<BookingDto>>(bookings);
            return await Responses.SuccessResponse(MappedBookings);
        }

        public async Task<Responses> UpdateBookingByPropertyId(int bookingId, BookingToUpdateDTO bookDto)
        {
            var booking = await _unitOfWork.Repository<Booking, int>().GetByIdAsync(bookingId);
            if (booking is null) return await Responses.FailurResponse("There is no bookgin with this id");

            bool isAvailable = await _unitOfWork.Repository<Booking, int>().CheckAvailabilityAsync(b => b.PropertyId == booking.PropertyId && b.Id != bookingId, booking.StartDate, booking.EndDate);
            if (!isAvailable) return await Responses.FailurResponse("please check another date, property isn't availabe at this time!");

            var totalPrice = (bookDto.EndDate.Day - bookDto.StartDate.Day) * booking.Property.NightPrice;
            booking.StartDate = bookDto.StartDate;
            booking.EndDate = bookDto.EndDate;
            booking.TotalPrice = totalPrice;

            _unitOfWork.Repository<Booking, int>().Update(booking);
            var Result = await _unitOfWork.CompleteAsync();
            if (Result <= 0) return await Responses.FailurResponse("Error has been occured while updating the booking!");
            return await Responses.SuccessResponse("Booking has been updated successfully!");
        }
    }
}
