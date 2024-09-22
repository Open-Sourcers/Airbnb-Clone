using Airbnb.Domain.DataTransferObjects.User;
using FluentValidation;

namespace Airbnb.APIs.Validators
{
    public class LoginValidator:AbstractValidator<LoginDTO>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email)
                        .NotEmpty().WithMessage("Email is required.")
                        .EmailAddress().WithMessage("Invalid email address format.");

            RuleFor(x => x.Password)
                        .NotEmpty().WithMessage("Password is required.")
                        .Matches(@"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,20}$")
                        .WithMessage("Password must be 8-20 characters long, with at least one uppercase letter, one digit, and one special character.");
        }
    }
}
