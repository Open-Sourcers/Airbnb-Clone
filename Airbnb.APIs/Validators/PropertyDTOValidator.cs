using System.Data;
using Airbnb.Domain.DataTransferObjects.Property;
using FluentValidation;

namespace Airbnb.APIs.Validators
{
    public class PropertyDTOValidator : AbstractValidator<PropertyRequest>
    {
        public PropertyDTOValidator()
        {
            RuleFor(x => x.Name).NotEmpty().Length(3, 50);
            RuleFor(x => x.Description).NotEmpty().Length(5,500);
            RuleFor(x => x.NightPrice).GreaterThan(0);
            RuleFor(x => x.PlaceType).NotEmpty();

            RuleForEach(x => x.RoomServices).NotEmpty();
            RuleForEach(x => x.Categories).NotEmpty();

            RuleFor(x => x.OwnereEmail).NotEmpty().EmailAddress();
            RuleFor(x => x.Location).NotNull();
            RuleFor(x => x.Region).NotNull();
            RuleFor(x => x.Country).NotNull();
            RuleForEach(x => x.Images).NotEmpty();
        }
    }
}
