using System.Data;
using Airbnb.Domain.DataTransferObjects;
using FluentValidation;

namespace Airbnb.APIs.Validators
{
    public class PropertyDTOValidator : AbstractValidator<PropertyDTO>
    {
        public PropertyDTOValidator()
        {
            RuleFor(p => p.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

            RuleFor(p => p.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

            RuleFor(p => p.NightPrice)
                .GreaterThan(0).WithMessage("NightPrice must be greater than zero.");

            RuleFor(p => p.PlaceType)
                .NotEmpty().WithMessage("PlaceType is required.")
                .MaximumLength(50).WithMessage("PlaceType must not exceed 50 characters.");

            RuleFor(p => p.ImageUrls)
                .Must(images => images != null && images.Any()).WithMessage("At least one image is required.");

            //RuleFor(p => p.Categories)
            //    .Must(categories => categories != null && categories.Any()).WithMessage("At least one category is required.");

            RuleFor(p => p.RoomServices)
                .Must(roomServices => roomServices != null && roomServices.Any()).WithMessage("At least one room service is required.");
        }
    }
}
