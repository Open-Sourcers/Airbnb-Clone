using Airbnb.Domain.DataTransferObjects;
using FluentValidation;

namespace Airbnb.APIs.Validators
{
    public class PropertyToCreateDTOValidator : AbstractValidator<PropertyToCreateDTO>
    {
        public PropertyToCreateDTOValidator()
        {
            // Name validation: required and between 2 and 100 characters
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Length(2, 100).WithMessage("Name must be between 2 and 100 characters.");

            // Description validation: required and at least 10 characters
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MinimumLength(10).WithMessage("Description must be at least 10 characters long.");

            // NightPrice validation: must be greater than 0
            RuleFor(x => x.NightPrice)
                .GreaterThan(0).WithMessage("Night price must be greater than 0.");

            // PlaceType validation: required and must be a specific type if needed (e.g., "House", "Apartment")
            RuleFor(x => x.PlaceType)
                .NotEmpty().WithMessage("Place type is required.");

            // Location validation: ensure LocationDto is not null
            RuleFor(x => x.Location)
                .NotNull().WithMessage("Location is required.");
            // Region validation: ensure RegionDto is not null
            RuleFor(x => x.Region)
                .NotNull().WithMessage("Region is required.");
            // Country validation: ensure CountryDto is not null
            RuleFor(x => x.Country)
                .NotNull().WithMessage("Country is required.");
            // Owner validation: ensure OwnerDto is not null
            RuleFor(x => x.Owner)
                .NotNull().WithMessage("Owner is required.");
            // Images validation: ensure there is at least one image
            RuleFor(x => x.Images)
                .NotEmpty().WithMessage("At least one image is required.");

            //// Categories validation: ensure there is at least one category
            //RuleFor(x => x.Categories)
            //    .NotEmpty().WithMessage("At least one category is required.");

            // Room Services validation: can be optional, but if provided, validate each room service
            RuleFor(x => x.RoomServices);
        }
    }
}
