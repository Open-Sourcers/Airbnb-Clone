using Airbnb.Domain.DataTransferObjects.User;
using FluentValidation;

namespace Airbnb.APIs.Validators
{
    public class ResetPasswordDTOValidator : AbstractValidator<ResetPasswordDTO>
    {
        public ResetPasswordDTOValidator()
        {
            RuleFor(x => x.Otp) .NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Token).NotEmpty();
            RuleFor(x => x.NewPassword).NotEmpty().MinimumLength(8).Matches("[A-Z]").Matches("[a-z]").Matches("[0-9]").Matches("[^a-zA-Z0-9]"); 
            RuleFor(x => x.PasswordComfirmation) .NotEmpty().Equal(x => x.NewPassword);       
        }
    }
}
