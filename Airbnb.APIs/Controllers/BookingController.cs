using System.Net;
using System.Security.Claims;
using Airbnb.Domain;
using Airbnb.Domain.DataTransferObjects;
using Airbnb.Domain.Entities;
using Airbnb.Domain.Interfaces.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Airbnb.APIs.Controllers
{
    public class BookingController : APIBaseController
    {
        private readonly IBookService _bookService;
        private readonly IValidator<BookingToCreateDTO> _bookingToCreateValidator;
        private readonly IPaymentService _paymentService;

        public BookingController(IBookService bookService, 
                                 IValidator<BookingToCreateDTO> bookingToCreateValidator,
                                 IPaymentService paymentService)
        {
            _bookService = bookService;
            _bookingToCreateValidator = bookingToCreateValidator;
            _paymentService = paymentService;
        }
        //[Authorize]
        [HttpPost("CreateBooking")]
        public async Task<ActionResult<Responses>> CreateBooking([FromBody] BookingToCreateDTO bookDTO)
        {
            var validate = await _bookingToCreateValidator.ValidateAsync(bookDTO);
            if (!validate.IsValid) return await Responses.FailurResponse(validate.Errors, HttpStatusCode.BadRequest);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (email is null)
            {
                return await Responses.FailurResponse("email is not found try again");
            }

            return Ok(await _bookService.CreateBookingByPropertyId(email, bookDTO));
        }

        [HttpPut("CancelBooking")]
        public async Task<ActionResult<Responses>> CancelBooking([FromQuery] int bookingId)
        {
            var Response = await _paymentService.PaymentCancelAsync(bookingId);
            return Ok(Response);
        }

         [HttpDelete("DeleteBooking")]
        public async Task<ActionResult<Responses>> DeleteBooking([FromQuery] int bookingId)
        {
            var Response = await _bookService.DeleteBookingById(bookingId);
            return Ok(Response);
        }


        [HttpGet("GetBooking")]
        public async Task<ActionResult<Responses>> GetBookingById([FromQuery] int bookingId)
        {
            return Ok(await _bookService.GetBookingById(bookingId));
        }


        [HttpGet("GetBookingsByUser")]
        public async Task<ActionResult<Responses>> GetBookingsByUserId([FromQuery] string userId)
        {
            return Ok(await _bookService.GetBookingsByUserId(userId));
        }

        [HttpPut("UpdateBookingById")]
        public async Task<ActionResult<Responses>> UpdateBookingByPropertyId(int bookingId, BookingToUpdateDTO bookDto)
        {
            return Ok(await _bookService.UpdateBookingByPropertyId(bookingId, bookDto));
        }
    }
}
