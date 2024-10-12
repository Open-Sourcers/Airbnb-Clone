using Airbnb.Domain.DataTransferObjects;
using FluentValidation;

namespace Airbnb.APIs.Validators
{
    public class ForgetPasswordValidation:AbstractValidator<ForgetPasswordDto>
    {
        public ForgetPasswordValidation()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }
}
