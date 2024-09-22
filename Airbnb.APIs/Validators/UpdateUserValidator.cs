using Airbnb.Domain.DataTransferObjects.User;
using Airbnb.Domain.Identity;
using FluentValidation;

namespace Airbnb.APIs.Validators
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserDTO>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.FirstName).Length(2, 20);
            RuleFor(x => x.MiddlName).MaximumLength(20);
            RuleFor(x => x.LastName).Length(2, 20);
            RuleFor(x=>x.UserName).Length(3,20);
            RuleFor(x => x.Address).Length(3,100);
            RuleFor(x => x.PhoneNumber).NotEmpty().Length(11,13);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }
}
