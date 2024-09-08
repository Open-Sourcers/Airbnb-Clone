using Airbnb.Domain.DataTransferObjects;
using FluentValidation;

namespace Airbnb.APIs.Validators
{
    public class ResetPasswordDTOValidator : AbstractValidator<ResetPasswordDTO>
    {
        public ResetPasswordDTOValidator()
        {
            RuleFor(x => x.CurrentPassword)
                .NotEmpty()
                .MinimumLength(6);

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .MinimumLength(6)
                .Matches("[A-Z]")  
                .Matches("[a-z]")  
                .Matches("[0-9]")  
                .Matches("[^a-zA-Z0-9]"); 

            RuleFor(x => x.NewPasswordComfirmation)
                .NotEmpty()
                .Equal(x => x.NewPassword);
        }
    }
}
