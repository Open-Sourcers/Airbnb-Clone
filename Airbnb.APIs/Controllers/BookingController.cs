using Airbnb.Application.Features.Bookings.Command;
using Airbnb.Application.Features.Bookings.Command.BookingCancelation;
using Airbnb.Application.Features.Bookings.Command.CreateBooking;
using Airbnb.Application.Features.Bookings.Query;
using Airbnb.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Airbnb.APIs.Controllers
{
    public class BookingController:APIBaseController
    {
        private readonly IMediator _mediator;

        public BookingController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("GetBookingById/{bookingId}")]
        public async Task<ActionResult<Responses>> GetBookingById(string bookingId)
        {
            var query=new GetBookingQuery(bookingId);
            return Ok(await _mediator.Send(query));
        }

        [HttpPost("reateBooking")]
        public async Task<ActionResult<Responses>> CreateBooking(CreateBookingCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPut("UpdateBooking")]
        public async Task<ActionResult<Responses>> GetUserBookings(UpdateBookingCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpDelete("DeleteBooking/{bookingId}")]
        public async Task<ActionResult<Responses>> BookingCancelation(string bookingId)
        {
            var command = new BookingCancelationCommand(bookingId);
            return Ok(await _mediator.Send(command));
        }



    }
}
