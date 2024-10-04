using Airbnb.Domain.DataTransferObjects;
using FluentValidation;

namespace Airbnb.APIs.Validators
{
    public class BookingToCreateDTOValidator : AbstractValidator<BookingToCreateDTO>
    {
        public BookingToCreateDTOValidator()
        {
            // Rule to ensure StartDate is in the future
            RuleFor(x => x.StartDate)
                .GreaterThan(DateTimeOffset.Now)
                .WithMessage("StartDate must be in the future.");

            // Rule to ensure EndDate is after StartDate
            RuleFor(x => x.EndDate)
                .GreaterThan(x => x.StartDate)
                .WithMessage("EndDate must be after StartDate.");

            // Rule to ensure PropertyId is not null or empty
            RuleFor(x => x.PropertyId)
                .NotEmpty()
                .WithMessage("PropertyId is required.");
        }
    }
}
