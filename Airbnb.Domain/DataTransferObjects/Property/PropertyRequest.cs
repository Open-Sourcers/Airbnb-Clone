
using Microsoft.AspNetCore.Http;
namespace Airbnb.Domain.DataTransferObjects.Property
{
    public class PropertyRequest:PropertyDto
    {
        public string OwnereEmail { get; set; }
        public List<IFormFile>? Images { get; set; }
    }
}
