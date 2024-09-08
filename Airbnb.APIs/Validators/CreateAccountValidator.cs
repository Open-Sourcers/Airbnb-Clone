using Airbnb.Domain.DataTransferObjects;
using FluentValidation;

namespace Airbnb.APIs.Validators
{
    public class CreateAccountValidator:AbstractValidator<RegisterDTO>
    {
        public CreateAccountValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .Length(2, 50).WithMessage("First name must be between 2 and 50 characters.");

            RuleFor(x => x.MiddlName)
                .MaximumLength(50).WithMessage("Middle name cannot exceed 50 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .Length(2, 50).WithMessage("Last name must be between 2 and 50 characters.");

            RuleFor(x => x.Address)
                .MaximumLength(100).WithMessage("Address cannot exceed 100 characters.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .MaximumLength(13)
                .MinimumLength(11);
          
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email address format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .Matches(@"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,20}$")
                .WithMessage("Password must be 8-20 characters long, with at least one uppercase letter, one digit, and one special character.");

            RuleFor(x => x.role)
                        .Must(role => role == Role.Customer|| role == Role.Owner)
                        .WithMessage("Invalid role. Allowed roles are 'Customer' and 'Owner'.");
        }
    }
}
