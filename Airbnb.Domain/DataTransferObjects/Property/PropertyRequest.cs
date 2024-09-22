using Airbnb.Domain.DataTransferObjects.Country;
using Airbnb.Domain.DataTransferObjects.Image;
using Airbnb.Domain.DataTransferObjects.Location;
using Airbnb.Domain.DataTransferObjects.Region;
namespace Airbnb.Domain.DataTransferObjects.Property
{
    public class PropertyRequest:PropertyDto
    {
        public string OwnereEmail { get; set; }
        public IEnumerable<ImageRequest> Images { get; set; }
    }
}
