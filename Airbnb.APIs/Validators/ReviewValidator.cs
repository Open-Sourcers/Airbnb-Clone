using Airbnb.Domain.DataTransferObjects;
using FluentValidation;

namespace Airbnb.APIs.Validators
{
    public class ReviewValidator : AbstractValidator<ReviewDto>
    {
        public ReviewValidator()
        {
            RuleFor(x => x.UserId).NotNull();
            RuleFor(x => x.PropertyId).NotNull();
            RuleFor(x => x.Comment).NotNull().Length(3, 500);
            RuleFor(x => x.Stars).NotNull().GreaterThanOrEqualTo(0).LessThanOrEqualTo(5);
        }
    }
}
