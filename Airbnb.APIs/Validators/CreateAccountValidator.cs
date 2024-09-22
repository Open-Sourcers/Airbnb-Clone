using Airbnb.Domain.DataTransferObjects.User;
using FluentValidation;

namespace Airbnb.APIs.Validators
{
    public class CreateAccountValidator : AbstractValidator<RegisterDTO>
    {
        public CreateAccountValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().Length(2, 50);
            RuleFor(x => x.MiddlName).MaximumLength(50);
            RuleFor(x => x.LastName).NotEmpty().Length(2, 50);
            RuleFor(x => x.Address).MaximumLength(100);
            RuleFor(x => x.PhoneNumber).NotEmpty().MaximumLength(13).MinimumLength(11);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty().Matches(@"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,20}$");
            RuleFor(x => x.roles).NotEmpty().Must(roles => roles.All(role => role == Role.Customer || role == Role.Owner));
        }
    }

}